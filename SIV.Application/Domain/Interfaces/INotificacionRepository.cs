using SIV.Application.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Application.Domain.Interfaces
{
    public interface INotificacionRepository : IBaseRepository<Notificacion>
    {
        Task<List<Notificacion>> BuscarPorUsuario(Guid usuarioId);

        Task<int> ContarNoLeidasAsync(Guid usuarioId);

        Task MarcarTodasComoLeidas(Guid usuarioId);
    }
}
