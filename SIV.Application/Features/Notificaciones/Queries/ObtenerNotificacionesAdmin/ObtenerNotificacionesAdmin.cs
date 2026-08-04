using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Notificacion;
using SIV.Domain.Common;
using SIV.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Notificaciones.Queries.ObtenerNotificacionesAdmin
{
    /// <summary>
    /// Consulta para obtener las notificaciones con filtros administrativos.
    /// </summary>
    public class ObtenerNotificacionesAdminQuery : IRequest<Result<List<NotificacionAdminDTO>>>
    {
        public Guid? VueloId { get; set; }
        public string? NumeroVuelo { get; set; }
        public Guid? UsuarioId { get; set; }
        public string? EmailUsuario { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? Leida { get; set; }
    }

    /// <summary>
    /// Validador para ObtenerNotificacionesAdminQuery.
    /// </summary>
    public class ObtenerNotificacionesAdminValidator : AbstractValidator<ObtenerNotificacionesAdminQuery>
    {
        public ObtenerNotificacionesAdminValidator()
        {
            RuleFor(x => x.FechaInicio).LessThanOrEqualTo(x => x.FechaFin)
                .WithMessage("La fecha de inicio no puede ser posterior a la fecha de fin.");
        }
    }

    /// <summary>
    /// Manejador para ObtenerNotificacionesAdminQuery.
    /// </summary>
    public class ObtenerNotificacionesAdminHandler : IRequestHandler<ObtenerNotificacionesAdminQuery, Result<List<NotificacionAdminDTO>>>
    {
        private readonly INotificacionRepository _notificacionRepository;
        private readonly ILogger<ObtenerNotificacionesAdminHandler> _logger;
        private readonly IValidator<ObtenerNotificacionesAdminQuery> _validator;

        public ObtenerNotificacionesAdminHandler(
            INotificacionRepository notificacionRepository,
            ILogger<ObtenerNotificacionesAdminHandler> logger,
            IValidator<ObtenerNotificacionesAdminQuery> validator)
        {
            _notificacionRepository = notificacionRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<List<NotificacionAdminDTO>>> Handle(ObtenerNotificacionesAdminQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<List<NotificacionAdminDTO>>();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Message = "Errores de validación: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return result;
                }

                var filtros = new FiltrosNotificaciones
                {
                    VueloId = request.VueloId,
                    NumeroVuelo = request.NumeroVuelo,
                    UsuarioId = request.UsuarioId,
                    EmailUsuario = request.EmailUsuario,
                    FechaInicio = request.FechaInicio,
                    FechaFin = request.FechaFin,
                    Leida = request.Leida
                };

                var notificaciones = (await _notificacionRepository.BuscarConFiltrosAsync(filtros)).ToList();

                List<NotificacionAdminDTO> listadoDTO = new();

                foreach (var notificacion in notificaciones)
                {
                    listadoDTO.Add(new NotificacionAdminDTO
                    {
                        Id = notificacion.Id,
                        VueloId = notificacion.VueloId,
                        UsuarioId = notificacion.UsuarioId,
                        NumeroVuelo = notificacion.Vuelo?.NumeroVuelo,
                        EmailUsuario = notificacion.Usuario?.Email,
                        Titulo = notificacion.Titulo,
                        Mensaje = notificacion.Mensaje,
                        FechaEnvio = notificacion.FechaEnvio,
                        Leida = notificacion.Leida
                    });
                }

                result.Success = true;
                result.Data = listadoDTO;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al obtener notificaciones administrativas.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al obtener las notificaciones.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener notificaciones administrativas.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al consultar las notificaciones.";
                return result;
            }
        }
    }
}
