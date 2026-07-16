using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Auditoria;
using SIV.Application.Common.Extensions;
using SIV.Application.Common.Models;
using SIV.Domain.Emuns;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Vuelos.Commands.CambiarEstadoVuelo
{
    /// <summary>
    /// Comando para cambiar el estado de un vuelo.
    /// </summary>
    public class CambiarEstadoVueloCommand : IRequest<Result<bool>>
    {
        public Guid VueloId { get; set; }
        public EstadoVuelo NuevoEstado { get; set; }
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para CambiarEstadoVueloCommand.
    /// </summary>
    public class CambiarEstadoVueloValidator : AbstractValidator<CambiarEstadoVueloCommand>
    {
        public CambiarEstadoVueloValidator()
        {
            RuleFor(x => x.VueloId).NotEmpty().WithMessage("El ID del vuelo es requerido.");
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("El ID del usuario es requerido.");
            RuleFor(x => x.NuevoEstado).IsInEnum().WithMessage("El estado especificado no es válido.");
        }
    }

    /// <summary>
    /// Manejador para CambiarEstadoVueloCommand.
    /// </summary>
    public class CambiarEstadoVueloHandler : IRequestHandler<CambiarEstadoVueloCommand, Result<bool>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly IHistorialEstadoRepository _historialEstadoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CambiarEstadoVueloHandler> _logger;
        private readonly IValidator<CambiarEstadoVueloCommand> _validator;

        public CambiarEstadoVueloHandler(
            IVueloRepository vueloRepository,
            IHistorialEstadoRepository historialEstadoRepository,
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<CambiarEstadoVueloHandler> logger,
            IValidator<CambiarEstadoVueloCommand> validator)
        {
            _vueloRepository = vueloRepository;
            _historialEstadoRepository = historialEstadoRepository;
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(CambiarEstadoVueloCommand request, CancellationToken cancellationToken)
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

                var usuario = await _usuarioRepository.GetByIdAsync(request.EjecutadorId);
                if (usuario == null || !usuario.EsOperadorOAdministrador())
                {
                    result.Success = false;
                    result.Message = "Solo operadores o administradores pueden realizar esta acción.";
                    return result;
                }

                var vuelo = await _vueloRepository.GetByIdAsync(request.VueloId);
                if (vuelo == null)
                {
                    result.Success = false;
                    result.Message = "Vuelo no encontrado.";
                    return result;
                }

                vuelo.EstadoActual = request.NuevoEstado;
                _vueloRepository.Update(vuelo);

                await _historialEstadoRepository.AddAsync(new HistorialEstado
                {
                    VueloId = vuelo.Id,
                    Estado = request.NuevoEstado
                });

                await _auditoriaManager.Registrar(
                    usuario.Email,
                    Modulo.Vuelos,
                    TipoAccion.Actualizar,
                    $"Estado cambiado a {request.NuevoEstado}",
                    vuelo.Id,
                    vuelo.NumeroVuelo
                );

                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = true;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al cambiar estado del vuelo.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al cambiar el estado del vuelo.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al cambiar estado del vuelo.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al cambiar el estado del vuelo.";
                return result;
            }
        }
    }
}
