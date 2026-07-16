using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Notificacion;
using SIV.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Usuarios.Queries.ObtenerNotificaciones
{
    /// <summary>
    /// Consulta para obtener las notificaciones de un usuario.
    /// </summary>
    public class ObtenerNotificacionesQuery : IRequest<Result<List<NotificacionDTO>>>
    {
        public Guid UsuarioId { get; set; }
    }

    /// <summary>
    /// Validador para ObtenerNotificacionesQuery.
    /// </summary>
    public class ObtenerNotificacionesValidator : AbstractValidator<ObtenerNotificacionesQuery>
    {
        public ObtenerNotificacionesValidator()
        {
            RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Se requiere el ID del usuario.");
        }
    }

    /// <summary>
    /// Manejador para ObtenerNotificacionesQuery.
    /// </summary>
    public class ObtenerNotificacionesHandler : IRequestHandler<ObtenerNotificacionesQuery, Result<List<NotificacionDTO>>>
    {
        private readonly INotificacionRepository _notificacionRepository;
        private readonly ILogger<ObtenerNotificacionesHandler> _logger;
        private readonly IValidator<ObtenerNotificacionesQuery> _validator;

        public ObtenerNotificacionesHandler(
            INotificacionRepository notificacionRepository,
            ILogger<ObtenerNotificacionesHandler> logger,
            IValidator<ObtenerNotificacionesQuery> validator)
        {
            _notificacionRepository = notificacionRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<List<NotificacionDTO>>> Handle(ObtenerNotificacionesQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<List<NotificacionDTO>>();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Message = "Errores de validación: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return result;
                }

                // Materializa la colección inmediatamente para evitar deferred execution.
                var notificaciones = (await _notificacionRepository.BuscarPorUsuarioAsync(request.UsuarioId)).ToList();

                List<NotificacionDTO> listadoDTO = new();

                foreach (var notificacion in notificaciones)
                {
                    listadoDTO.Add(new NotificacionDTO
                    {
                        Id = notificacion.Id,
                        Titulo = notificacion.Titulo,
                        Mensaje = notificacion.Mensaje,
                        FechaEnvio = notificacion.FechaEnvio,
                        Leida = notificacion.Leida,
                        VueloId = notificacion.VueloId
                    });
                }

                result.Success = true;
                result.Data = listadoDTO;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al obtener notificaciones.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al obtener las notificaciones.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener notificaciones.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al consultar las notificaciones.";
                return result;
            }
        }
    }
}
