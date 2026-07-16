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
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Usuarios.Commands.SeguirVuelo
{
    /// <summary>
    /// Comando para seguir un vuelo.
    /// </summary>
    public class SeguirVueloCommand : IRequest<Result<bool>>
    {
        public Guid VueloId { get; set; }
        public Guid UsuarioId { get; set; }
    }

    /// <summary>
    /// Validador para SeguirVueloCommand.
    /// </summary>
    public class SeguirVueloValidator : AbstractValidator<SeguirVueloCommand>
    {
        public SeguirVueloValidator()
        {
            RuleFor(x => x.VueloId).NotEmpty().WithMessage("Se requiere el ID del vuelo.");
            RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Se requiere el ID del usuario.");
        }
    }

    /// <summary>
    /// Manejador para SeguirVueloCommand.
    /// </summary>
    public class SeguirVueloHandler : IRequestHandler<SeguirVueloCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IVueloRepository _vueloRepository;
        private readonly ISeguimientoVueloRepository _seguimientoVueloRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SeguirVueloHandler> _logger;
        private readonly IValidator<SeguirVueloCommand> _validator;

        public SeguirVueloHandler(
            IUsuarioRepository usuarioRepository,
            IVueloRepository vueloRepository,
            ISeguimientoVueloRepository seguimientoVueloRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<SeguirVueloHandler> logger,
            IValidator<SeguirVueloCommand> validator)
        {
            _usuarioRepository = usuarioRepository;
            _vueloRepository = vueloRepository;
            _seguimientoVueloRepository = seguimientoVueloRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(SeguirVueloCommand request, CancellationToken cancellationToken)
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
                    result.Message = "Solo usuarios registrados pueden seguir vuelos.";
                    return result;
                }

                var vueloSeguir = await _vueloRepository.GetByIdAsync(request.VueloId);

                if (vueloSeguir == null)
                {
                    result.Success = false;
                    result.Message = "Vuelo no encontrado.";
                    return result;
                }

                if (vueloSeguir.EstadoActual == EstadoVuelo.Cancelado ||
                    vueloSeguir.EstadoActual == EstadoVuelo.Completado)
                {
                    await _auditoriaManager.Registrar(
                        usuarioAutenticado.Email,
                        Modulo.Usuarios,
                        TipoAccion.SeguirVuelo,
                        "Error: Intento de seguir vuelo ya finalizado",
                        null,
                        usuarioAutenticado.Email
                    );
                    await _unitOfWork.SaveChangesAsync();

                    result.Success = false;
                    result.Message = "No se puede seguir este vuelo (ya finalizado).";
                    return result;
                }

                bool yaSigue = await _seguimientoVueloRepository
                    .ExisteSeguimiento(usuarioAutenticado.Id, vueloSeguir.Id);

                if (yaSigue)
                {
                    await _auditoriaManager.Registrar(
                        usuarioAutenticado.Email,
                        Modulo.Usuarios,
                        TipoAccion.SeguirVuelo,
                        "Error: intento de seguir vuelo ya seguido",
                        null,
                        usuarioAutenticado.Email
                    );
                    await _unitOfWork.SaveChangesAsync();

                    result.Success = false;
                    result.Message = "Ya estás siguiendo este vuelo.";
                    return result;
                }

                var seguimiento = new SeguimientoVuelo
                {
                    UsuarioId = usuarioAutenticado.Id,
                    VueloId = vueloSeguir.Id,
                    FechaInicio = DateTime.Now
                };

                await _seguimientoVueloRepository.AddAsync(seguimiento);

                await _auditoriaManager.Registrar(
                        usuarioAutenticado.Email,
                        Modulo.Usuarios,
                        TipoAccion.SeguirVuelo,
                        "Seguimiento iniciado con exito",
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
                _logger.LogError(ex, "Error de base de datos al seguir el vuelo.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al procesar el seguimiento del vuelo.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al seguir el vuelo.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al intentar seguir el vuelo.";
                return result;
            }
        }
    }
}
