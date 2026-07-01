using SIV.Domain.Emuns;

namespace SIV.Application.Auditoria
{
    public interface IAuditoriaManager
    {
        Task Registrar(
                string actor,
                Modulo modulo,
                TipoAccion accion,
                string resultado,
                Guid? entidadAfectadaId,
                string descripcionEntidad);

    }
}
