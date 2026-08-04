using MediatR;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.DTOs.Fids;
using SIV.Application.Features.Fids.Queries.GetFidsVuelos;
using SIV.Domain.Emuns;
using SIV.Presentation.WebApi.Common;

namespace SIV.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class FidsController : ControllerBase
    {
        private readonly ISender _sender;

        public FidsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("vuelos")]
        [ProducesResponseType(typeof(List<DtoFidsVuelo>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVuelos(
            [FromQuery] TipoPantallaFids tipoPantalla,
            [FromQuery] string? aeropuerto,
            [FromQuery] EstadoVuelo? estado,
            [FromQuery] Guid? aerolineaId,
            [FromQuery] TimeSpan? rangoHoras)
        {
            var result = await _sender.Send(new GetFidsVuelosQuery
            {
                TipoPantalla = tipoPantalla,
                AeropuertoCodigo = aeropuerto,
                Estado = estado,
                AerolineaId = aerolineaId,
                RangoHoras = rangoHoras
            });

            if (!result.Success) return BadRequest(ApiResponse.Error(result.Message));
            return Ok(result.Data);
        }
    }
}
