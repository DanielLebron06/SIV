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

namespace SIV.Application.Features.Usuarios.Commands.DesactivarUsuario
{
    /// <summary>
    /// Comando para desactivar un usuario.
    /// </summary>
    public class DesactivarUsuarioCommand : IRequest<Result<bool>>
    {
        public Guid IdUsuarioADesactivar { get; set; }
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para DesactivarUsuarioCommand.
    /// </summary>
    public class DesactivarUsuarioValidator : AbstractValidator<DesactivarUsuarioCommand>
    {
        public DesactivarUsuarioValidator()
        {
            RuleFor(x => x.IdUsuarioADesactivar).NotEmpty().WithMessage("Se requiere el ID del usuario a desactivar.");
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("Se requiere el ID del usuario que ejecuta la acción.");
        }
    }

    /// <summary>
    /// Manejador para DesactivarUsuarioCommand.
    /// </summary>
    public class DesactivarUsuarioHandler : IRequestHandler<DesactivarUsuarioCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DesactivarUsuarioHandler> _logger;
        private readonly IValidator<DesactivarUsuarioCommand> _validator;

        public DesactivarUsuarioHandler(
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<DesactivarUsuarioHandler> logger,
            IValidator<DesactivarUsuarioCommand> validator)
        {
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(DesactivarUsuarioCommand request, CancellationToken cancellationToken)
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
                    result.Message = "Solo un administrador puede desactivar usuario.";
                    return result;
                }

                var user = await _usuarioRepository.GetByIdAsync(request.IdUsuarioADesactivar);

                if (user == null)
                {
                    await _auditoriaManager.Registrar(
                        ejecutador.Email,
                        Modulo.Usuarios,
                        TipoAccion.DesactivarUsuario,
                        "Error: usuario no encontrado",
                        null,
                        request.IdUsuarioADesactivar.ToString()
                    );
                    await _unitOfWork.SaveChangesAsync();

                    result.Success = false;
                    result.Message = "Usuario no encontrado.";
                    return result;
                }

                if (!user.Activo)
                {
                    await _auditoriaManager.Registrar(
                        ejecutador.Email,
                        Modulo.Usuarios,
                        TipoAccion.DesactivarUsuario,
                        "Intento de desactivar usuario ya inactivo",
                        user.Id,
                        user.Email
                    );
                    await _unitOfWork.SaveChangesAsync();

                    result.Success = false;
                    result.Message = "El usuario ya está desactivado.";
                    return result;
                }

                user.Activo = false;

                _usuarioRepository.Update(user);

                await _auditoriaManager.Registrar(
                    ejecutador.Email,
                    Modulo.Usuarios,
                    TipoAccion.DesactivarUsuario,
                    "Usuario desactivado",
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
                _logger.LogError(ex, "Error de base de datos al desactivar usuario.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al desactivar el usuario.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al desactivar usuario.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al intentar desactivar el usuario.";
                return result;
            }
        }
    }
}
