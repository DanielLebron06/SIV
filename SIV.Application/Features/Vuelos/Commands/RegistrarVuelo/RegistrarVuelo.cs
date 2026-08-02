using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SIV.Application.Auditoria;
using SIV.Application.Common.Extensions;
using SIV.Application.Common.Models;
using SIV.Application.Common.Settings;
using SIV.Application.DTOs.Vuelo;
using SIV.Domain.Emuns;
using SIV.Domain.Entities;
using SIV.Domain.Exceptions;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Vuelos.Commands.RegistrarVuelo
{
    /// <summary>
    /// Comando para registrar un nuevo vuelo.
    /// </summary>
    public class RegistrarVueloCommand : IRequest<Result<bool>>
    {
        public DatosVueloDTO Datos { get; set; } = new();
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para RegistrarVueloCommand.
    /// </summary>
    public class RegistrarVueloValidator : AbstractValidator<RegistrarVueloCommand>
    {
        public RegistrarVueloValidator()
        {
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("Se requiere el ID del usuario que ejecuta la acción.");
            RuleFor(x => x.Datos).NotNull().WithMessage("Los datos del vuelo son requeridos.");
            RuleFor(x => x.Datos.NumeroVuelo).NotEmpty().WithMessage("El número de vuelo es requerido.");
            RuleFor(x => x.Datos.AerolineaId).NotEmpty().WithMessage("La aerolínea es requerida.");
            RuleFor(x => x.Datos.AeropuertoOrigenId).NotEmpty().WithMessage("El aeropuerto de origen es requerido.");
            RuleFor(x => x.Datos.AeropuertoDestinoId).NotEmpty().WithMessage("El aeropuerto de destino es requerido.");
            RuleFor(x => x.Datos.FechaSalidaProgramada).NotEmpty().WithMessage("La fecha de salida es requerida.")
                .GreaterThan(DateTimeOffset.Now).WithMessage("La fecha de salida debe ser posterior a la fecha y hora actual.");
            RuleFor(x => x.Datos.FechaLlegadaProgramada).NotEmpty().WithMessage("La fecha de llegada es requerida.")
                .GreaterThan(x => x.Datos.FechaSalidaProgramada).WithMessage("La fecha de llegada debe ser posterior a la de salida.");
            RuleFor(x => x.Datos)
                .Must(d => d.AeropuertoOrigenId != d.AeropuertoDestinoId)
                .WithMessage("El aeropuerto de origen no puede ser el mismo que el de destino.");
        }
    }

    /// <summary>
    /// Manejador para RegistrarVueloCommand.
    /// </summary>
    public class RegistrarVueloHandler : IRequestHandler<RegistrarVueloCommand, Result<bool>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly IAerolineaRepository _aerolineaRepository;
        private readonly IAeropuertoRepository _aeropuertoRepository;
        private readonly IHistorialEstadoRepository _historialEstadoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegistrarVueloHandler> _logger;
        private readonly IValidator<RegistrarVueloCommand> _validator;
        private readonly IOptions<AppSettings> _appSettings;

        public RegistrarVueloHandler(
            IVueloRepository vueloRepository,
            IAerolineaRepository aerolineaRepository,
            IAeropuertoRepository aeropuertoRepository,
            IHistorialEstadoRepository historialEstadoRepository,
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<RegistrarVueloHandler> logger,
            IValidator<RegistrarVueloCommand> validator,
            IOptions<AppSettings> appSettings)
        {
            _vueloRepository = vueloRepository;
            _aerolineaRepository = aerolineaRepository;
            _aeropuertoRepository = aeropuertoRepository;
            _historialEstadoRepository = historialEstadoRepository;
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
            _appSettings = appSettings;
        }

        public async Task<Result<bool>> Handle(RegistrarVueloCommand request, CancellationToken cancellationToken)
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

                var existe = await _vueloRepository.ExisteDuplicadoAsync(
                    request.Datos.NumeroVuelo,
                    request.Datos.AerolineaId,
                    request.Datos.AeropuertoOrigenId,
                    request.Datos.AeropuertoDestinoId,
                    request.Datos.FechaSalidaProgramada);
                if (existe)
                {
                    result.Success = false;
                    result.Message = "Ya existe un vuelo con el mismo número, aerolínea, fecha de salida y ruta.";
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

                var aeropuertoLocalId = _appSettings.Value.AeropuertoLocalId;
                if (aeropuertoLocalId == Guid.Empty)
                {
                    result.Success = false;
                    result.Message = "El aeropuerto local no está configurado en el sistema.";
                    return result;
                }

                if (request.Datos.AeropuertoOrigenId != aeropuertoLocalId && request.Datos.AeropuertoDestinoId != aeropuertoLocalId)
                {
                    result.Success = false;
                    result.Message = "Al menos uno de los aeropuertos debe ser el aeropuerto local del sistema.";
                    return result;
                }

                var vuelo = Vuelo.Crear(
                    request.Datos.NumeroVuelo,
                    request.Datos.AerolineaId,
                    request.Datos.AeropuertoOrigenId,
                    request.Datos.AeropuertoDestinoId,
                    request.Datos.FechaSalidaProgramada,
                    request.Datos.FechaLlegadaProgramada,
                    usuario.Id);

                await _vueloRepository.AddAsync(vuelo);

                await _historialEstadoRepository.AddAsync(new HistorialEstado
                {
                    VueloId = vuelo.Id,
                    Estado = EstadoVuelo.Programado
                });

                await _auditoriaManager.Registrar(
                    usuario.Email,
                    Modulo.Vuelos,
                    TipoAccion.Crear,
                    "Vuelo registrado",
                    vuelo.Id,
                    vuelo.NumeroVuelo
                );

                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = true;
                return result;
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Regla de negocio violada al registrar vuelo.");
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al registrar vuelo.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al registrar el vuelo.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar vuelo.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al registrar el vuelo.";
                return result;
            }
        }
    }
}
