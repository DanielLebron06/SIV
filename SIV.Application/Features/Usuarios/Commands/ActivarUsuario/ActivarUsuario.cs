using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Auditoria;
using SIV.Application.Common.Extensions;
using SIV.Application.Common.Models;
using SIV.Domain.Emuns;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Usuarios.Commands.ActivarUsuario
{
    /// <summary>
    /// Comando para activar un usuario.
    /// </summary>
    public class ActivarUsuarioCommand : IRequest<Result<bool>>
    {
        public Guid IdUsuarioAactivar { get; set; }
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para ActivarUsuarioCommand.
    /// </summary>
    public class ActivarUsuarioValidator : AbstractValidator<ActivarUsuarioCommand>
    {
        public ActivarUsuarioValidator()
        {
            RuleFor(x => x.IdUsuarioAactivar).NotEmpty().WithMessage("Se requiere el ID del usuario a activar.");
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("Se requiere el ID del usuario que ejecuta la acción.");
        }
    }

    /// <summary>
    /// Manejador para ActivarUsuarioCommand.
    /// </summary>
    public class ActivarUsuarioHandler : IRequestHandler<ActivarUsuarioCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ActivarUsuarioHandler> _logger;
        private readonly IValidator<ActivarUsuarioCommand> _validator;

        public ActivarUsuarioHandler(
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<ActivarUsuarioHandler> logger,
            IValidator<ActivarUsuarioCommand> validator)
        {
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(ActivarUsuarioCommand request, CancellationToken cancellationToken)
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
                    result.Message = "Solo un administrador puede Activar usuario.";
                    return result;
                }

                var user = await _usuarioRepository.GetByIdAsync(request.IdUsuarioAactivar);

                if (user == null)
                {
                    await _auditoriaManager.Registrar(
                        ejecutador.Email,
                        Modulo.Usuarios,
                        TipoAccion.ActivarUsuario,
                        "Error: usuario no encontrado",
                        null,
                        request.IdUsuarioAactivar.ToString()
                    );
                    await _unitOfWork.SaveChangesAsync();

                    result.Success = false;
                    result.Message = "Usuario no encontrado.";
                    return result;
                }

                if (user.Activo)
                {
                    await _auditoriaManager.Registrar(
                        ejecutador.Email,
                        Modulo.Usuarios,
                        TipoAccion.ActivarUsuario,
                        "Intento de activar usuario ya activo",
                        user.Id,
                        user.Email
                    );
                    await _unitOfWork.SaveChangesAsync();

                    result.Success = false;
                    result.Message = "El usuario ya está Activado.";
                    return result;
                }

                user.Activo = true;

                _usuarioRepository.Update(user);

                await _auditoriaManager.Registrar(
                    ejecutador.Email,
                    Modulo.Usuarios,
                    TipoAccion.ActivarUsuario,
                    "Usuario Activado",
                    user.Id,
                    user.Email
                );

                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = true;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al activar usuario.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al activar el usuario.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al activar usuario.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al intentar activar el usuario.";
                return result;
            }
        }
    }
}
