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

namespace SIV.Application.Features.Usuarios.Commands.RegistrarUsuarioPublico
{
    /// <summary>
    /// Comando para registrar un usuario público.
    /// </summary>
    public class RegistrarUsuarioPublicoCommand : IRequest<Result<bool>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Validador para RegistrarUsuarioPublicoCommand.
    /// </summary>
    public class RegistrarUsuarioPublicoValidator : AbstractValidator<RegistrarUsuarioPublicoCommand>
    {
        public RegistrarUsuarioPublicoValidator()
        {
            RuleFor(x => x.Email).Requerido("Email").EmailAddress().WithMessage("El email no tiene un formato válido.");
            RuleFor(x => x.Password).PasswordSeguro();
        }
    }

    /// <summary>
    /// Manejador para RegistrarUsuarioPublicoCommand.
    /// </summary>
    public class RegistrarUsuarioPublicoHandler : IRequestHandler<RegistrarUsuarioPublicoCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly ILogger<RegistrarUsuarioPublicoHandler> _logger;
        private readonly IValidator<RegistrarUsuarioPublicoCommand> _validator;

        public RegistrarUsuarioPublicoHandler(
            IUsuarioRepository usuarioRepository,
            IUnitOfWork unitOfWork,
            IAuditoriaManager auditoriaManager,
            ILogger<RegistrarUsuarioPublicoHandler> logger,
            IValidator<RegistrarUsuarioPublicoCommand> validator)
        {
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
            _auditoriaManager = auditoriaManager;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(RegistrarUsuarioPublicoCommand request, CancellationToken cancellationToken)
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

                var existe = await _usuarioRepository.BuscarPorEmail(request.Email);
                if (existe != null)
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
                    Rol = Rol.UsuarioRegistrado,
                    Activo = true
                };

                await _usuarioRepository.AddAsync(newUser);

                await _auditoriaManager.Registrar(
                    request.Email,
                    Modulo.Usuarios,
                    TipoAccion.Crear,
                    "Usuario publico registrado",
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
                _logger.LogError(ex, "Error de base de datos al registrar usuario público.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al registrar el usuario.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar usuario público.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al registrar el usuario.";
                return result;
            }
        }
    }
}
