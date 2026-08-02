using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Vuelo;
using SIV.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Vuelos.Queries.ConsultarHistorialCambios
{
    public class ConsultarHistorialCambiosQuery : IRequest<Result<List<CambioOperativoDTO>>>
    {
        public Guid VueloId { get; set; }
    }

    public class ConsultarHistorialCambiosHandler : IRequestHandler<ConsultarHistorialCambiosQuery, Result<List<CambioOperativoDTO>>>
    {
        private readonly ICambioOperativoRepository _cambioOperativoRepository;
        private readonly ILogger<ConsultarHistorialCambiosHandler> _logger;

        public ConsultarHistorialCambiosHandler(
            ICambioOperativoRepository cambioOperativoRepository,
            ILogger<ConsultarHistorialCambiosHandler> logger)
        {
            _cambioOperativoRepository = cambioOperativoRepository;
            _logger = logger;
        }

        public async Task<Result<List<CambioOperativoDTO>>> Handle(ConsultarHistorialCambiosQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<List<CambioOperativoDTO>>();
            try
            {
                var cambios = await _cambioOperativoRepository.GetAllAsync();
                var historial = cambios
                    .Where(c => c.VueloId == request.VueloId)
                    .OrderByDescending(c => c.Timestamp)
                    .Select(c => new CambioOperativoDTO
                    {
                        Id = c.Id,
                        VueloId = c.VueloId,
                        TipoCambio = c.TipoCambio,
                        Motivo = c.Motivo,
                        Timestamp = c.Timestamp,
                        UsuarioResponsableId = c.UsuarioResponsableId
                    })
                    .ToList();

                result.Success = true;
                result.Data = historial;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar historial de cambios.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado.";
                return result;
            }
        }
    }
}
