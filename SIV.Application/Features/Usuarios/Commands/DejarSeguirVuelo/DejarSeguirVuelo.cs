using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Auditoria;
using SIV.Application.Common.Extensions;
using SIV.Application.Common.Models;
using SIV.Domain.Emuns;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Usuarios.Commands.DejarSeguirVuelo
{
    /// <summary>
    /// Comando para dejar de seguir un vuelo.
    /// </summary>
    public class DejarSeguirVueloCommand : IRequest<Result<bool>>
    {
        public Guid VueloId { get; set; }
        public Guid UsuarioId { get; set; }
    }

    /// <summary>
    /// Validador para DejarSeguirVueloCommand.
    /// </summary>
    public class DejarSeguirVueloValidator : AbstractValidator<DejarSeguirVueloCommand>
    {
        public DejarSeguirVueloValidator()
        {
            RuleFor(x => x.VueloId).NotEmpty().WithMessage("Se requiere el ID del vuelo.");
            RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Se requiere el ID del usuario.");
        }
    }

    /// <summary>
    /// Manejador para DejarSeguirVueloCommand.
    /// </summary>
    public class DejarSeguirVueloHandler : IRequestHandler<DejarSeguirVueloCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IVueloRepository _vueloRepository;
        private readonly ISeguimientoVueloRepository _seguimientoVueloRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DejarSeguirVueloHandler> _logger;
        private readonly IValidator<DejarSeguirVueloCommand> _validator;

        public DejarSeguirVueloHandler(
            IUsuarioRepository usuarioRepository,
            IVueloRepository vueloRepository,
            ISeguimientoVueloRepository seguimientoVueloRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<DejarSeguirVueloHandler> logger,
            IValidator<DejarSeguirVueloCommand> validator)
        {
            _usuarioRepository = usuarioRepository;
            _vueloRepository = vueloRepository;
            _seguimientoVueloRepository = seguimientoVueloRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(DejarSeguirVueloCommand request, CancellationToken cancellationToken)
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

                var usuarioAutenticado = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
                if (usuarioAutenticado == null || !usuarioAutenticado.EsUsuarioRegistrado())
                {
                    result.Success = false;
                    result.Message = "Solo usuarios registrados pueden dejar de seguir vuelos.";
                    return result;
                }

                var vueloSeguir = await _vueloRepository.GetByIdAsync(request.VueloId);

                if (vueloSeguir == null)
                {
                    result.Success = false;
                    result.Message = "Vuelo no encontrado.";
                    return result;
                }

                bool yaSigue = await _seguimientoVueloRepository
                   .ExisteSeguimiento(usuarioAutenticado.Id, vueloSeguir.Id);

                if (!yaSigue)
                {
                    await _auditoriaManager.Registrar(
                        usuarioAutenticado.Email,
                        Modulo.Usuarios,
                        TipoAccion.DejarSeguirVuelo,
                        "Error: intento de dejar de seguir vuelo no seguido",
                        null,
                        vueloSeguir.NumeroVuelo.ToString()
                    );
                    await _unitOfWork.SaveChangesAsync();

                    result.Success = false;
                    result.Message = "No estas siguiendo este vuelo.";
                    return result;
                }

                var seguimiento = await _seguimientoVueloRepository
                    .ObtenerSeguimiento(usuarioAutenticado.Id, request.VueloId);

                if (seguimiento == null)
                {
                    result.Success = false;
                    result.Message = "Seguimiento no encontrado.";
                    return result;
                }

                seguimiento.FechaFin = DateTime.Now;

                _seguimientoVueloRepository.Update(seguimiento);

                await _auditoriaManager.Registrar(
                        usuarioAutenticado.Email,
                        Modulo.Usuarios,
                        TipoAccion.DejarSeguirVuelo,
                        "Seguimiento cancelado",
                        seguimiento.Id,
                        vueloSeguir.NumeroVuelo.ToString()
                    );

                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = true;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al dejar de seguir el vuelo.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al procesar la cancelación del seguimiento.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al dejar de seguir el vuelo.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al intentar dejar de seguir el vuelo.";
                return result;
            }
        }
    }
}
