using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Auditoria;
using SIV.Application.Common.Extensions;
using SIV.Application.Common.Models;
using SIV.Application.Common.Security;
using SIV.Application.DTOs.Usuario;
using SIV.Domain.Emuns;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Usuarios.Commands.IniciarSesion
{
    /// <summary>
    /// Comando para iniciar sesión.
    /// </summary>
    public class IniciarSesionCommand : IRequest<Result<UsuarioDTO>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Validador para IniciarSesionCommand.
    /// </summary>
    public class IniciarSesionValidator : AbstractValidator<IniciarSesionCommand>
    {
        public IniciarSesionValidator()
        {
            RuleFor(x => x.Email).Requerido("Email").EmailAddress().WithMessage("El email no tiene un formato válido.");
            RuleFor(x => x.Password).Requerido("Password");
        }
    }

    /// <summary>
    /// Manejador para IniciarSesionCommand.
    /// </summary>
    public class IniciarSesionHandler : IRequestHandler<IniciarSesionCommand, Result<UsuarioDTO>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly ILogger<IniciarSesionHandler> _logger;
        private readonly IValidator<IniciarSesionCommand> _validator;
        // NOTA: Se asume que IUnitOfWork inyectado también maneja SaveChanges de Auditoria si se encola.
        // En los servicios viejos se llamaba SaveChangesAsync.
        private readonly SIV.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public IniciarSesionHandler(
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            ILogger<IniciarSesionHandler> logger,
            IValidator<IniciarSesionCommand> validator,
            SIV.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _logger = logger;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UsuarioDTO>> Handle(IniciarSesionCommand request, CancellationToken cancellationToken)
        {
            var result = new Result<UsuarioDTO>();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Message = "Errores de validación: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return result;
                }

                // Lógica legacy para el admin hardcodeado.
                if (request.Email == "admin@siv.com" && request.Password == "123")
                {
                    var adminUser = await _usuarioRepository.BuscarPorEmail(request.Email);
                    if (adminUser != null)
                    {
                        result.Success = true;
                        result.Data = new UsuarioDTO { Id = adminUser.Id, Email = adminUser.Email, Rol = adminUser.Rol };
                        return result;
                    }
                }

                var userRegistrado = await _usuarioRepository.BuscarPorEmail(request.Email);

                if (userRegistrado == null)
                {
                    await _auditoriaManager.Registrar(
                        request.Email,
                        Modulo.Usuarios,
                        TipoAccion.Login,
                        "Error: usuario no encontrado",
                        null,
                        request.Email
                    );

                    await _unitOfWork.SaveChangesAsync();
                    
                    result.Success = false;
                    result.Message = "Credenciales inválidas.";
                    return result;
                }

                bool esValido = PasswordHasher.VerifyPassword(request.Password, userRegistrado.PasswordHash);

                if (!esValido)
                {
                    await _auditoriaManager.Registrar(
                        userRegistrado.Email,
                        Modulo.Usuarios,
                        TipoAccion.Login,
                        "Error: Contraseña invalida",
                        userRegistrado.Id,
                        userRegistrado.Email);

                    await _unitOfWork.SaveChangesAsync();

                    result.Success = false;
                    result.Message = "Credenciales inválidas.";
                    return result;
                }

                if (!userRegistrado.Activo)
                {
                    result.Success = false;
                    result.Message = "El usuario se encuentra inactivo.";
                    return result;
                }

                await _auditoriaManager.Registrar(
                        request.Email,
                        Modulo.Usuarios,
                        TipoAccion.Login,
                        "Inicio de sesion realizado con exito",
                        userRegistrado.Id,
                        userRegistrado.Email);

                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = new UsuarioDTO
                {
                    Id = userRegistrado.Id,
                    Email = userRegistrado.Email,
                    Rol = userRegistrado.Rol
                };

                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al iniciar sesión.");
                result.Success = false;
                result.Message = "Ocurrió un error de base de datos.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al iniciar sesión.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al intentar iniciar sesión.";
                return result;
            }
        }
    }
}
