using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Auditoria;
using SIV.Application.Common.Extensions;
using SIV.Application.Common.Models;
using SIV.Domain.Emuns;
using SIV.Domain.Exceptions;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Vuelos.Commands.AsignarPuertaInicial
{
    /// <summary>
    /// Comando para la primera asignación de puerta de embarque de un vuelo.
    /// No registra un cambio operativo en la tabla CambiosOperativos; solo actualiza
    /// el vuelo y genera una entrada en el log de auditoría.
    /// </summary>
    public class AsignarPuertaInicialCommand : IRequest<Result<bool>>
    {
        public Guid VueloId { get; set; }
        public string Puerta { get; set; } = string.Empty;
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para AsignarPuertaInicialCommand.
    /// </summary>
    public class AsignarPuertaInicialValidator : AbstractValidator<AsignarPuertaInicialCommand>
    {
        public AsignarPuertaInicialValidator()
        {
            RuleFor(x => x.VueloId).NotEmpty().WithMessage("El ID del vuelo es requerido.");
            RuleFor(x => x.Puerta).NotEmpty().WithMessage("La puerta de embarque es requerida.");
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("El ID del usuario es requerido.");
        }
    }

    /// <summary>
    /// Manejador para AsignarPuertaInicialCommand.
    /// </summary>
    public class AsignarPuertaInicialHandler : IRequestHandler<AsignarPuertaInicialCommand, Result<bool>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AsignarPuertaInicialHandler> _logger;
        private readonly IValidator<AsignarPuertaInicialCommand> _validator;

        public AsignarPuertaInicialHandler(
            IVueloRepository vueloRepository,
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<AsignarPuertaInicialHandler> logger,
            IValidator<AsignarPuertaInicialCommand> validator)
        {
            _vueloRepository = vueloRepository;
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(AsignarPuertaInicialCommand request, CancellationToken cancellationToken)
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

                vuelo.AsignarPuertaInicial(request.Puerta);
                _vueloRepository.Update(vuelo);

                await _auditoriaManager.Registrar(
                    usuario.Email,
                    Modulo.Vuelos,
                    TipoAccion.Crear,
                    $"Puerta {request.Puerta.Trim()} asignada al vuelo {vuelo.NumeroVuelo}.",
                    vuelo.Id,
                    vuelo.NumeroVuelo
                );

                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = true;
                return result;
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Regla de negocio violada al asignar la puerta inicial.");
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al asignar la puerta inicial.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al asignar la puerta inicial.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado.";
                return result;
            }
        }
    }
}
