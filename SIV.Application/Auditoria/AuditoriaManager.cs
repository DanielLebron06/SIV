

using SIV.Domain.Emuns;
using SIV.Domain.Entities;
using SIV.Domain.Repositories;

namespace SIV.Application.Auditoria
{
    public class AuditoriaManager : IAuditoriaManager
    {

        private readonly IBaseRepository<LogAuditoria> _logRepository;

        public AuditoriaManager(IBaseRepository<LogAuditoria> logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task Registrar(
            string actor,
            Modulo modulo,
            TipoAccion accion,
            string resultado,
            Guid? entidadAfectadaId,
            string descripcionEntidad)
        {
            var log = new LogAuditoria
            {
                Actor = actor,
                Modulo = modulo,
                TipoAccion = accion,
                Resultado = resultado,
                EntidadAfectadaId = entidadAfectadaId,
                DescripcionEntidad = descripcionEntidad
            };

            await _logRepository.AddAsync(log);
        }

    }
}
