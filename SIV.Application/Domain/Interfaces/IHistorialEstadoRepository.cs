using SIV.Application.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Application.Domain.Interfaces
{
    public interface IHistorialEstadoRepository : IBaseRepository<HistorialEstado>
    {
        Task<List<HistorialEstado>> BuscarPorVuelo(Guid VueloID);
    }
}
