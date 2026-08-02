using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Auditoria;
using SIV.Application.Common.Extensions;
using SIV.Application.Common.Models;
using SIV.Domain.Emuns;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Vuelos.Commands.CancelarVuelo
{
    public class CancelarVueloCommand : IRequest<Result<bool>>
    {
        public Guid VueloId { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public Guid EjecutadorId { get; set; }
    }

    public class CancelarVueloValidator : AbstractValidator<CancelarVueloCommand>
    {
        public CancelarVueloValidator()
        {
            RuleFor(x => x.VueloId).NotEmpty().WithMessage("El ID del vuelo es requerido.");
            RuleFor(x => x.Motivo).NotEmpty().WithMessage("El motivo de cancelación es obligatorio.");
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("El ID del usuario es requerido.");
        }
    }

    public class CancelarVueloHandler : IRequestHandler<CancelarVueloCommand, Result<bool>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ICambioOperativoRepository _cambioOperativoRepository;
        private readonly IHistorialEstadoRepository _historialEstadoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CancelarVueloHandler> _logger;
        private readonly IValidator<CancelarVueloCommand> _validator;

        public CancelarVueloHandler(
            IVueloRepository vueloRepository,
            ICambioOperativoRepository cambioOperativoRepository,
            IHistorialEstadoRepository historialEstadoRepository,
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<CancelarVueloHandler> logger,
            IValidator<CancelarVueloCommand> validator)
        {
            _vueloRepository = vueloRepository;
            _cambioOperativoRepository = cambioOperativoRepository;
            _historialEstadoRepository = historialEstadoRepository;
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(CancelarVueloCommand request, CancellationToken cancellationToken)
        {
            var result = new Result<bool>();
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Message = "Errores de validación: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return result;
                }

                var usuario = await _usuarioRepository.GetByIdAsync(request.EjecutadorId);
                if (usuario == null || !usuario.EsOperadorOAdministrador())
                {
                    result.Success = false;
                    result.Message = "Solo operadores o administradores pueden realizar esta acción.";
                    return result;
                }

                var vuelo = await _vueloRepository.GetByIdAsync(request.VueloId);
                if (vuelo == null)
                {
                    result.Success = false;
                    result.Message = "Vuelo no encontrado.";
                    return result;
                }

                if (vuelo.EstadoActual == EstadoVuelo.Cancelado || vuelo.EstadoActual == EstadoVuelo.Completado)
                {
                    result.Success = false;
                    result.Message = "Un vuelo ya Cancelado o Completado no puede ser cancelado nuevamente.";
                    return result;
                }

                vuelo.EstadoActual = EstadoVuelo.Cancelado;
                _vueloRepository.Update(vuelo);

                await _historialEstadoRepository.AddAsync(new HistorialEstado
                {
                    VueloId = vuelo.Id,
                    Estado = EstadoVuelo.Cancelado
                });

                await _cambioOperativoRepository.AddAsync(new CambioOperativo
                {
                    VueloId = vuelo.Id,
                    TipoCambio = TipoCambio.Cancelacion,
                    Motivo = request.Motivo,
                    UsuarioResponsableId = request.EjecutadorId
                });

                await _auditoriaManager.Registrar(
                    usuario.Email,
                    Modulo.Vuelos,
                    TipoAccion.Actualizar,
                    $"Vuelo cancelado. Motivo: {request.Motivo}",
                    vuelo.Id,
                    vuelo.NumeroVuelo
                );

                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = true;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al cancelar el vuelo.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al cancelar el vuelo.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado.";
                return result;
            }
        }
    }
}
