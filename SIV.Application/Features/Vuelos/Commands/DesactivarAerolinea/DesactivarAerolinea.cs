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

namespace SIV.Application.Features.Vuelos.Commands.DesactivarAerolinea
{
    /// <summary>
    /// Comando para desactivar una aerolínea.
    /// </summary>
    public class DesactivarAerolineaCommand : IRequest<Result<bool>>
    {
        public Guid AerolineaId { get; set; }
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para DesactivarAerolineaCommand.
    /// </summary>
    public class DesactivarAerolineaValidator : AbstractValidator<DesactivarAerolineaCommand>
    {
        public DesactivarAerolineaValidator()
        {
            RuleFor(x => x.AerolineaId).NotEmpty().WithMessage("El ID de la aerolínea es requerido.");
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("El ID del usuario es requerido.");
        }
    }

    /// <summary>
    /// Manejador para DesactivarAerolineaCommand.
    /// </summary>
    public class DesactivarAerolineaHandler : IRequestHandler<DesactivarAerolineaCommand, Result<bool>>
    {
        private readonly IAerolineaRepository _aerolineaRepository;
        private readonly IVueloRepository _vueloRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DesactivarAerolineaHandler> _logger;
        private readonly IValidator<DesactivarAerolineaCommand> _validator;

        public DesactivarAerolineaHandler(
            IAerolineaRepository aerolineaRepository,
            IVueloRepository vueloRepository,
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<DesactivarAerolineaHandler> logger,
            IValidator<DesactivarAerolineaCommand> validator)
        {
            _aerolineaRepository = aerolineaRepository;
            _vueloRepository = vueloRepository;
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(DesactivarAerolineaCommand request, CancellationToken cancellationToken)
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
                if (usuario == null || !usuario.EsAdministrador())
                {
                    result.Success = false;
                    result.Message = "Solo un administrador puede realizar esta acción.";
                    return result;
                }

                var aerolinea = await _aerolineaRepository.GetByIdAsync(request.AerolineaId);
                if (aerolinea == null)
                {
                    result.Success = false;
                    result.Message = "Aerolínea no encontrada.";
                    return result;
                }

                if (!aerolinea.Activo)
                {
                    result.Success = false;
                    result.Message = "La aerolínea ya está desactivada.";
                    return result;
                }

                var tieneVuelosActivos = await _vueloRepository.ExistenVuelosActivosPorAerolineaAsync(request.AerolineaId);
                if (tieneVuelosActivos)
                {
                    result.Success = false;
                    result.Message = "No se puede desactivar la aerolínea porque tiene vuelos activos asociados.";
                    return result;
                }

                aerolinea.Activo = false;
                _aerolineaRepository.Update(aerolinea);

                await _auditoriaManager.Registrar(
                    usuario.Email,
                    Modulo.Vuelos,
                    TipoAccion.Eliminar,
                    "Aerolínea desactivada",
                    aerolinea.Id,
                    aerolinea.Nombre
                );

                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = true;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al desactivar aerolínea.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al desactivar la aerolínea.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al desactivar aerolínea.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al desactivar la aerolínea.";
                return result;
            }
        }
    }
}
