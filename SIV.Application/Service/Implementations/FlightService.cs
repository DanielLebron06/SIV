
using SIV.Application.Auditoria;
using SIV.Application.DTOs.Aerolinea;
using SIV.Application.DTOs.Aeropuerto;
using SIV.Application.DTOs.Vuelo;
using SIV.Application.Service.Interfaces;
using SIV.Domain.Common;
using SIV.Domain.Emuns;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;

namespace SIV.Application.Service.Implementations
{
    public class FlightService : IFlightService
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly IAerolineaRepository _aerolineaRepository;
        private readonly IAeropuertoRepository _aeropuertoRepository;
        private readonly IHistorialEstadoRepository _historialEstadoRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;

        public FlightService(
            IVueloRepository vueloRepository,
            IAerolineaRepository aerolineaRepository,
            IAeropuertoRepository aeropuertoRepository,
            IHistorialEstadoRepository historialEstadoRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork)
        {
            _vueloRepository = vueloRepository;
            _aerolineaRepository = aerolineaRepository;
            _aeropuertoRepository = aeropuertoRepository;
            _historialEstadoRepository = historialEstadoRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
        }

        private void ValidarAdministrador(Usuario usuario)
        {
            if (usuario.Rol != Rol.Administrador)
            {
                throw new Exception("Solo un administrador puede realizar esta acción");
            }
        }

        private void ValidarOperadorOAdministrador(Usuario usuario)
        {
            if (usuario.Rol != Rol.Operador &&
                usuario.Rol != Rol.Administrador)
            {
                throw new Exception(
                    "Solo operadores o administradores pueden realizar esta acción");
            }
        }

        private async Task<Vuelo> ObtenerVuelo(Guid vueloId)
        {
            var vuelo = await _vueloRepository.GetByIdAsync(vueloId);

            if (vuelo == null)
            {
                throw new Exception("Vuelo no encontrado");
            }

            return vuelo;
        }

        private async Task<Aerolinea> ObtenerAerolinea(Guid aerolineaId)
        {
            var aerolinea = await _aerolineaRepository.GetByIdAsync(aerolineaId);

            if (aerolinea == null)
            {
                throw new Exception("Aerolínea no encontrada");
            }

            return aerolinea;
        }

        private async Task<Aeropuerto> ObtenerAeropuerto(Guid aeropuertoId)
        {
            var aeropuerto = await _aeropuertoRepository.GetByIdAsync(aeropuertoId);

            if (aeropuerto == null)
            {
                throw new Exception("Aeropuerto no encontrado");
            }

            return aeropuerto;
        }

        private async Task RegistrarHistorialEstado(
            Guid vueloId,
            EstadoVuelo estado)
        {
            await _historialEstadoRepository.AddAsync(
                new HistorialEstado
                {
                    VueloId = vueloId,
                    Estado = estado
                });
        }

        public async Task RegistrarVuelo(DatosVueloDTO datos, Usuario usuario)
        {
            ValidarOperadorOAdministrador(usuario);

            var existe = await _vueloRepository
                .BuscarPorNumeroVuelo(datos.NumeroVuelo);

            if (existe != null)
            {
                throw new Exception("Ya existe un vuelo con ese número");
            }

            await ObtenerAerolinea(datos.AerolineaId);
            await ObtenerAeropuerto(datos.AeropuertoOrigenId);
            await ObtenerAeropuerto(datos.AeropuertoDestinoId);

            var vuelo = new Vuelo
            {
                NumeroVuelo = datos.NumeroVuelo,
                AerolineaId = datos.AerolineaId,
                AeropuertoOrigenId = datos.AeropuertoOrigenId,
                AeropuertoDestinoId = datos.AeropuertoDestinoId,
                EstadoActual = EstadoVuelo.Programado,
                SalidaPlanificada = datos.FechaSalidaProgramada,
                LlegadaPlanificada = datos.FechaLlegadaProgramada,
                CreadoPorId = usuario.Id
            };

            await _vueloRepository.AddAsync(vuelo);

            await RegistrarHistorialEstado(
                vuelo.Id,
                EstadoVuelo.Programado);

            await _auditoriaManager.Registrar(
                usuario.Email,
                Modulo.Vuelos,
                TipoAccion.Crear,
                "Vuelo registrado",
                vuelo.Id,
                vuelo.NumeroVuelo
            );

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ActualizarVuelo(
    Guid vueloId,
    DatosVueloDTO datos,
    Usuario usuario)
        {
            ValidarOperadorOAdministrador(usuario);

            var vuelo = await ObtenerVuelo(vueloId);

            await ObtenerAerolinea(datos.AerolineaId);
            await ObtenerAeropuerto(datos.AeropuertoOrigenId);
            await ObtenerAeropuerto(datos.AeropuertoDestinoId);

            vuelo.NumeroVuelo = datos.NumeroVuelo;
            vuelo.AerolineaId = datos.AerolineaId;
            vuelo.AeropuertoOrigenId = datos.AeropuertoOrigenId;
            vuelo.AeropuertoDestinoId = datos.AeropuertoDestinoId;
            vuelo.SalidaPlanificada = datos.FechaSalidaProgramada;
            vuelo.LlegadaPlanificada = datos.FechaLlegadaProgramada;

            _vueloRepository.Update(vuelo);

            await _auditoriaManager.Registrar(
                usuario.Email,
                Modulo.Vuelos,
                TipoAccion.Actualizar,
                "Vuelo actualizado",
                vuelo.Id,
                vuelo.NumeroVuelo
            );

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task RegistrarAerolinea(RegistroAerolineaDTO datos, Usuario usuario)
        {
            ValidarAdministrador(usuario);

            var existe = await _aerolineaRepository
                .BuscarPorCodigoAsync(datos.CodigoIATA);

            if (existe != null)
            {
                throw new Exception("Ya existe una aerolínea con ese código");
            }

            var aerolinea = new Aerolinea
            {
                Nombre = datos.Nombre,
                CodigoIATA = datos.CodigoIATA
            };

            await _aerolineaRepository.AddAsync(aerolinea);

            await _auditoriaManager.Registrar(
                usuario.Email,
                Modulo.Vuelos,
                TipoAccion.Crear,
                "Aerolínea creada",
                aerolinea.Id,
                aerolinea.Nombre
            );

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RegistrarAeropuerto(RegistroAeropuertoDTO datos, Usuario usuario)
        {
            ValidarAdministrador(usuario);

            var existe = await _aeropuertoRepository
                .BuscarPorCodigoAsync(datos.CodigoIATA);

            if (existe != null)
            {
                throw new Exception("Ya existe un aeropuerto con ese código");
            }

            var aeropuerto = new Aeropuerto
            {
                Nombre = datos.Nombre,
                CodigoIATA = datos.CodigoIATA,
                Ciudad = datos.Ciudad,
                Pais = datos.Pais
            };

            await _aeropuertoRepository.AddAsync(aeropuerto);

            await _auditoriaManager.Registrar(
                usuario.Email,
                Modulo.Vuelos,
                TipoAccion.Crear,
                "Aeropuerto creado",
                aeropuerto.Id,
                aeropuerto.Nombre
            );

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<AerolineaDTO>> ObtenerAerolineas(Usuario usuario)
        {
            var aerolineas = await _aerolineaRepository
                .ObtenerActivosAsync();

            List<AerolineaDTO> resultado = new();

            foreach (var aerolinea in aerolineas)
            {
                resultado.Add(new AerolineaDTO
                {
                    Id = aerolinea.Id,
                    Nombre = aerolinea.Nombre,
                    Activa = aerolinea.Activo
                });
            }

            return resultado;
        }

        public async Task<List<AeropuertoDTO>> ObtenerAeropuertos(Usuario usuario)
        {
            var aeropuertos = await _aeropuertoRepository
                .ObtenerActivosAsync();

            List<AeropuertoDTO> resultado = new();

            foreach (var aeropuerto in aeropuertos)
            {
                resultado.Add(new AeropuertoDTO
                {
                    Id = aeropuerto.Id,
                    Nombre = aeropuerto.Nombre,
                    CodigoIATA = aeropuerto.CodigoIATA,
                    Activo = aeropuerto.Activo
                });
            }

            return resultado;
        }

        public async Task DesactivarAerolinea(
    Guid aerolineaId,
    Usuario usuario)
        {
            ValidarAdministrador(usuario);

            var aerolinea = await ObtenerAerolinea(aerolineaId);

            if (!aerolinea.Activo)
            {
                throw new Exception("La aerolínea ya está desactivada");
            }

            aerolinea.Activo = false;

            _aerolineaRepository.Update(aerolinea);

            await _auditoriaManager.Registrar(
                usuario.Email,
                Modulo.Vuelos,
                TipoAccion.Eliminar,
                "Aerolínea desactivada",
                aerolinea.Id,
                aerolinea.Nombre
            );

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DesactivarAeropuerto(
    Guid aeropuertoId,
    Usuario usuario)
        {
            ValidarAdministrador(usuario);

            var aeropuerto = await ObtenerAeropuerto(aeropuertoId);

            if (!aeropuerto.Activo)
            {
                throw new Exception("El aeropuerto ya está desactivado");
            }

            aeropuerto.Activo = false;

            _aeropuertoRepository.Update(aeropuerto);

            await _auditoriaManager.Registrar(
                usuario.Email,
                Modulo.Vuelos,
                TipoAccion.Eliminar,
                "Aeropuerto desactivado",
                aeropuerto.Id,
                aeropuerto.Nombre
            );

            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<List<DatosVueloDTO>> ConsultarVuelos(
    FiltrosVuelos filtros,
    Usuario usuario)
        {
            var vuelos = await _vueloRepository
                .BuscarConFiltros(filtros);

            List<DatosVueloDTO> resultado = new();

            foreach (var vuelo in vuelos)
            {
                resultado.Add(new DatosVueloDTO
                {
                    Id = vuelo.Id,
                    NumeroVuelo = vuelo.NumeroVuelo,
                    AerolineaId = vuelo.AerolineaId,
                    AeropuertoOrigenId = vuelo.AeropuertoOrigenId,
                    AeropuertoDestinoId = vuelo.AeropuertoDestinoId,
                    EstadoActual = vuelo.EstadoActual,
                    FechaSalidaProgramada = vuelo.SalidaPlanificada,
                    FechaLlegadaProgramada = vuelo.LlegadaPlanificada
                });
            }

            return resultado;
        }

        public async Task CambiarEstadoVuelo(
    Guid vueloId,
    EstadoVuelo nuevoEstado,
    Usuario usuario)
        {
            ValidarOperadorOAdministrador(usuario);

            var vuelo = await ObtenerVuelo(vueloId);

            vuelo.EstadoActual = nuevoEstado;

            _vueloRepository.Update(vuelo);

            await RegistrarHistorialEstado(
                vuelo.Id,
                nuevoEstado);

            await _auditoriaManager.Registrar(
                usuario.Email,
                Modulo.Vuelos,
                TipoAccion.Actualizar,
                $"Estado cambiado a {nuevoEstado}",
                vuelo.Id,
                vuelo.NumeroVuelo
            );

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<HistorialEstadoDTO>> ObtenerEstadosVuelo(
    Guid vueloId,
    Usuario usuario)
        {
            var historial = await _historialEstadoRepository
                .ObtenerPorVueloAsync(vueloId);

            List<HistorialEstadoDTO> resultado = new();

            foreach (var estado in historial)
            {
                resultado.Add(new HistorialEstadoDTO
                {
                    Id = estado.Id,
                    Estado = estado.Estado,
                    FechaCambio = estado.FechaTransicion
                });
            }

            return resultado;
        }



    }
}
