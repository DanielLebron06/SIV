using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Auditoria;
using SIV.Application.Common.Extensions;
using SIV.Application.Common.Models;
using SIV.Domain.Emuns;
using SIV.Domain.Entities;
using SIV.Domain.Exceptions;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Vuelos.Commands.RegistrarRetraso
{
    public class RegistrarRetrasoCommand : IRequest<Result<bool>>
    {
        public Guid VueloId { get; set; }
        public DateTimeOffset NuevaHoraEstimada { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public Guid EjecutadorId { get; set; }
    }

    public class RegistrarRetrasoValidator : AbstractValidator<RegistrarRetrasoCommand>
    {
        public RegistrarRetrasoValidator()
        {
            RuleFor(x => x.VueloId).NotEmpty().WithMessage("El ID del vuelo es requerido.");
            RuleFor(x => x.NuevaHoraEstimada).NotEmpty().WithMessage("La nueva hora estimada es requerida.");
            RuleFor(x => x.Motivo).NotEmpty().WithMessage("El motivo del retraso es obligatorio.");
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("El ID del usuario es requerido.");
        }
    }

    public class RegistrarRetrasoHandler : IRequestHandler<RegistrarRetrasoCommand, Result<bool>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ICambioOperativoRepository _cambioOperativoRepository;
        private readonly IHistorialEstadoRepository _historialEstadoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly ISeguimientoVueloRepository _seguimientoVueloRepository;
        private readonly INotificacionRepository _notificacionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegistrarRetrasoHandler> _logger;
        private readonly IValidator<RegistrarRetrasoCommand> _validator;

        public RegistrarRetrasoHandler(
            IVueloRepository vueloRepository,
            ICambioOperativoRepository cambioOperativoRepository,
            IHistorialEstadoRepository historialEstadoRepository,
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            ISeguimientoVueloRepository seguimientoVueloRepository,
            INotificacionRepository notificacionRepository,
            IUnitOfWork unitOfWork,
            ILogger<RegistrarRetrasoHandler> logger,
            IValidator<RegistrarRetrasoCommand> validator)
        {
            _vueloRepository = vueloRepository;
            _cambioOperativoRepository = cambioOperativoRepository;
            _historialEstadoRepository = historialEstadoRepository;
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _seguimientoVueloRepository = seguimientoVueloRepository;
            _notificacionRepository = notificacionRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(RegistrarRetrasoCommand request, CancellationToken cancellationToken)
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

                vuelo.RegistrarRetraso(request.NuevaHoraEstimada);
                _vueloRepository.Update(vuelo);

                await _historialEstadoRepository.AddAsync(new HistorialEstado
                {
                    VueloId = vuelo.Id,
                    Estado = EstadoVuelo.Retrasado
                });

                await _cambioOperativoRepository.AddAsync(new CambioOperativo
                {
                    VueloId = vuelo.Id,
                    TipoCambio = TipoCambio.Retraso,
                    Motivo = request.Motivo,
                    UsuarioResponsableId = request.EjecutadorId
                });

                await _auditoriaManager.Registrar(
                    usuario.Email,
                    Modulo.Vuelos,
                    TipoAccion.Actualizar,
                    $"Vuelo retrasado a {request.NuevaHoraEstimada}. Motivo: {request.Motivo}",
                    vuelo.Id,
                    vuelo.NumeroVuelo
                );

                await CrearNotificacionesAsync(vuelo, $"Vuelo retrasado", $"El vuelo {vuelo.NumeroVuelo} fue retrasado a las {request.NuevaHoraEstimada.ToLocalTime():HH:mm}.");

                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = true;
                return result;
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Regla de negocio violada al registrar el retraso.");
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al registrar el retraso.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar el retraso.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado.";
                return result;
            }
        }

        private async Task CrearNotificacionesAsync(Vuelo vuelo, string titulo, string mensaje)
        {
            var seguidores = await _seguimientoVueloRepository.ObtenerSeguidoresPorVueloIdAsync(vuelo.Id);
            foreach (var seguidor in seguidores)
            {
                await _notificacionRepository.AddAsync(new Notificacion
                {
                    VueloId = vuelo.Id,
                    UsuarioId = seguidor.UsuarioId,
                    Titulo = titulo,
                    Mensaje = mensaje
                });
            }
        }
    }
}
