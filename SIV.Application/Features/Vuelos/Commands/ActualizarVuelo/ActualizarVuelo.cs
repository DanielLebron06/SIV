using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Auditoria;
using SIV.Application.Common.Extensions;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Vuelo;
using SIV.Domain.Emuns;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Vuelos.Commands.ActualizarVuelo
{
    /// <summary>
    /// Comando para actualizar un vuelo.
    /// </summary>
    public class ActualizarVueloCommand : IRequest<Result<bool>>
    {
        public Guid VueloId { get; set; }
        public DatosVueloDTO Datos { get; set; } = new();
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para ActualizarVueloCommand.
    /// </summary>
    public class ActualizarVueloValidator : AbstractValidator<ActualizarVueloCommand>
    {
        public ActualizarVueloValidator()
        {
            RuleFor(x => x.VueloId).NotEmpty().WithMessage("El ID del vuelo es requerido.");
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("Se requiere el ID del usuario que ejecuta la acción.");
            RuleFor(x => x.Datos).NotNull().WithMessage("Los datos del vuelo son requeridos.");
            RuleFor(x => x.Datos.NumeroVuelo).NotEmpty().WithMessage("El número de vuelo es requerido.");
        }
    }

    /// <summary>
    /// Manejador para ActualizarVueloCommand.
    /// </summary>
    public class ActualizarVueloHandler : IRequestHandler<ActualizarVueloCommand, Result<bool>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly IAerolineaRepository _aerolineaRepository;
        private readonly IAeropuertoRepository _aeropuertoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ActualizarVueloHandler> _logger;
        private readonly IValidator<ActualizarVueloCommand> _validator;

        public ActualizarVueloHandler(
            IVueloRepository vueloRepository,
            IAerolineaRepository aerolineaRepository,
            IAeropuertoRepository aeropuertoRepository,
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<ActualizarVueloHandler> logger,
            IValidator<ActualizarVueloCommand> validator)
        {
            _vueloRepository = vueloRepository;
            _aerolineaRepository = aerolineaRepository;
            _aeropuertoRepository = aeropuertoRepository;
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(ActualizarVueloCommand request, CancellationToken cancellationToken)
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

                var aerolinea = await _aerolineaRepository.GetByIdAsync(request.Datos.AerolineaId);
                if (aerolinea == null)
                {
                    result.Success = false;
                    result.Message = "Aerolínea no encontrada.";
                    return result;
                }

                var aeropuertoOrigen = await _aeropuertoRepository.GetByIdAsync(request.Datos.AeropuertoOrigenId);
                if (aeropuertoOrigen == null)
                {
                    result.Success = false;
                    result.Message = "Aeropuerto de origen no encontrado.";
                    return result;
                }

                var aeropuertoDestino = await _aeropuertoRepository.GetByIdAsync(request.Datos.AeropuertoDestinoId);
                if (aeropuertoDestino == null)
                {
                    result.Success = false;
                    result.Message = "Aeropuerto de destino no encontrado.";
                    return result;
                }

                vuelo.NumeroVuelo = request.Datos.NumeroVuelo;
                vuelo.AerolineaId = request.Datos.AerolineaId;
                vuelo.AeropuertoOrigenId = request.Datos.AeropuertoOrigenId;
                vuelo.AeropuertoDestinoId = request.Datos.AeropuertoDestinoId;
                vuelo.SalidaPlanificada = request.Datos.FechaSalidaProgramada;
                vuelo.LlegadaPlanificada = request.Datos.FechaLlegadaProgramada;

                _vueloRepository.Update(vuelo);

                await _auditoriaManager.Registrar(
                    usuario.Email,
                    Modulo.Vuelos,
                    TipoAccion.Actualizar,
                    "Vuelo actualizado",
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
                _logger.LogError(ex, "Error de base de datos al actualizar vuelo.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al actualizar el vuelo.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar vuelo.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al actualizar el vuelo.";
                return result;
            }
        }
    }
}
