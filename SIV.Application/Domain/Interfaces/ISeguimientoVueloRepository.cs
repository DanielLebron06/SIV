using SIV.Application.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Application.Domain.Interfaces
{
    public interface ISeguimientoVueloRepository : IBaseRepository<SeguimientoVuelo>
    {
        Task<List<SeguimientoVuelo>> BuscarPorUsuario(Guid usuarioId);

        Task<List<SeguimientoVuelo>> BuscarActivosPorUsuario(Guid usuarioId);

        Task<List<SeguimientoVuelo>> BuscarInactivosPorUsuario(Guid usuarioId);

        Task<bool> ExisteSeguimiento(Guid usuarioId, Guid vueloId);
    }
}
