using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Auditoria;
using SIV.Application.Common.Extensions;
using SIV.Application.Common.Models;
using SIV.Application.Common.Security;
using SIV.Domain.Emuns;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Usuarios.Commands.RegistrarUsuarioInterno
{
    /// <summary>
    /// Comando para registrar un usuario interno (solo administradores).
    /// </summary>
    public class RegistrarUsuarioInternoCommand : IRequest<Result<bool>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Rol Rol { get; set; }
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para RegistrarUsuarioInternoCommand.
    /// </summary>
    public class RegistrarUsuarioInternoValidator : AbstractValidator<RegistrarUsuarioInternoCommand>
    {
        public RegistrarUsuarioInternoValidator()
        {
            RuleFor(x => x.Email).Requerido("Email").EmailAddress().WithMessage("El email no tiene un formato válido.");
            RuleFor(x => x.Password).PasswordSeguro();
            RuleFor(x => x.Rol).IsInEnum().WithMessage("Rol inválido.");
        }
    }

    /// <summary>
    /// Manejador para RegistrarUsuarioInternoCommand.
    /// </summary>
    public class RegistrarUsuarioInternoHandler : IRequestHandler<RegistrarUsuarioInternoCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly ILogger<RegistrarUsuarioInternoHandler> _logger;
        private readonly IValidator<RegistrarUsuarioInternoCommand> _validator;

        public RegistrarUsuarioInternoHandler(
            IUsuarioRepository usuarioRepository,
            IUnitOfWork unitOfWork,
            IAuditoriaManager auditoriaManager,
            ILogger<RegistrarUsuarioInternoHandler> logger,
            IValidator<RegistrarUsuarioInternoCommand> validator)
        {
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
            _auditoriaManager = auditoriaManager;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(RegistrarUsuarioInternoCommand request, CancellationToken cancellationToken)
        {
            var result = new Result<bool>();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Message = "Errores de validación: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return result;
                }

                var ejecutador = await _usuarioRepository.GetByIdAsync(request.EjecutadorId);
                if (ejecutador == null || !ejecutador.EsAdministrador())
                {
                    result.Success = false;
                    result.Message = "Solo un administrador puede crear usuarios internos.";
                    return result;
                }

                if (request.Rol != Rol.Operador && request.Rol != Rol.Auditor)
                {
                    result.Success = false;
                    result.Message = "Rol interno inválido. Debe ser Operador o Auditor.";
                    return result;
                }

                var existe = await _usuarioRepository.ExistePorEmailAsync(request.Email);
                if (existe)
                {
                    await _auditoriaManager.Registrar(
                        request.Email,
                        Modulo.Usuarios,
                        TipoAccion.Crear,
                        "Error: email ya registrado",
                        null,
                        request.Email
                    );

                    result.Success = false;
                    result.Message = "El email ya está registrado.";
                    return result;
                }

                var newUser = new Usuario
                {
                    Email = request.Email,
                    PasswordHash = PasswordHasher.HashPassword(request.Password),
                    Rol = request.Rol,
                    Activo = true
                };

                await _usuarioRepository.AddAsync(newUser);

                await _auditoriaManager.Registrar(
                    ejecutador.Email,
                    Modulo.Usuarios,
                    TipoAccion.Crear,
                    "Usuario interno creado",
                    newUser.Id,
                    newUser.Email
                );

                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = true;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al registrar usuario interno.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al registrar el usuario interno.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar usuario interno.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al registrar el usuario interno.";
                return result;
            }
        }
    }
}
