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

namespace SIV.Application.Features.Vuelos.Commands.DesactivarAeropuerto
{
    /// <summary>
    /// Comando para desactivar un aeropuerto.
    /// </summary>
    public class DesactivarAeropuertoCommand : IRequest<Result<bool>>
    {
        public Guid AeropuertoId { get; set; }
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para DesactivarAeropuertoCommand.
    /// </summary>
    public class DesactivarAeropuertoValidator : AbstractValidator<DesactivarAeropuertoCommand>
    {
        public DesactivarAeropuertoValidator()
        {
            RuleFor(x => x.AeropuertoId).NotEmpty().WithMessage("El ID del aeropuerto es requerido.");
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("El ID del usuario es requerido.");
        }
    }

    /// <summary>
    /// Manejador para DesactivarAeropuertoCommand.
    /// </summary>
    public class DesactivarAeropuertoHandler : IRequestHandler<DesactivarAeropuertoCommand, Result<bool>>
    {
        private readonly IAeropuertoRepository _aeropuertoRepository;
        private readonly IVueloRepository _vueloRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DesactivarAeropuertoHandler> _logger;
        private readonly IValidator<DesactivarAeropuertoCommand> _validator;

        public DesactivarAeropuertoHandler(
            IAeropuertoRepository aeropuertoRepository,
            IVueloRepository vueloRepository,
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<DesactivarAeropuertoHandler> logger,
            IValidator<DesactivarAeropuertoCommand> validator)
        {
            _aeropuertoRepository = aeropuertoRepository;
            _vueloRepository = vueloRepository;
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(DesactivarAeropuertoCommand request, CancellationToken cancellationToken)
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

                var aeropuerto = await _aeropuertoRepository.GetByIdAsync(request.AeropuertoId);
                if (aeropuerto == null)
                {
                    result.Success = false;
                    result.Message = "Aeropuerto no encontrado.";
                    return result;
                }

                if (!aeropuerto.Activo)
                {
                    result.Success = false;
                    result.Message = "El aeropuerto ya está desactivado.";
                    return result;
                }

                var tieneVuelosActivos = await _vueloRepository.ExistenVuelosActivosPorAeropuertoAsync(request.AeropuertoId);
                if (tieneVuelosActivos)
                {
                    result.Success = false;
                    result.Message = "No se puede desactivar el aeropuerto porque tiene vuelos activos asociados.";
                    return result;
                }

                aeropuerto.Activo = false;
                _aeropuertoRepository.Update(aeropuerto);

                await _auditoriaManager.Registrar(
                    usuario.Email,
                    Modulo.Vuelos,
                    TipoAccion.Eliminar,
                    "Aeropuerto desactivado",
                    aeropuerto.Id,
                    aeropuerto.Nombre
                );

                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = true;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al desactivar aeropuerto.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al desactivar el aeropuerto.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al desactivar aeropuerto.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al desactivar el aeropuerto.";
                return result;
            }
        }
    }
}
