using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Models;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Notificaciones.Commands.MarcarNotificacionLeida
{
    /// <summary>
    /// Comando para marcar una notificación como leída.
    /// </summary>
    public class MarcarNotificacionLeidaCommand : IRequest<Result<bool>>
    {
        public Guid NotificacionId { get; set; }
    }

    /// <summary>
    /// Validador para MarcarNotificacionLeidaCommand.
    /// </summary>
    public class MarcarNotificacionLeidaValidator : AbstractValidator<MarcarNotificacionLeidaCommand>
    {
        public MarcarNotificacionLeidaValidator()
        {
            RuleFor(x => x.NotificacionId).NotEmpty().WithMessage("El ID de la notificación es requerido.");
        }
    }

    /// <summary>
    /// Manejador para MarcarNotificacionLeidaCommand.
    /// </summary>
    public class MarcarNotificacionLeidaHandler : IRequestHandler<MarcarNotificacionLeidaCommand, Result<bool>>
    {
        private readonly INotificacionRepository _notificacionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MarcarNotificacionLeidaHandler> _logger;
        private readonly IValidator<MarcarNotificacionLeidaCommand> _validator;

        public MarcarNotificacionLeidaHandler(
            INotificacionRepository notificacionRepository,
            IUnitOfWork unitOfWork,
            ILogger<MarcarNotificacionLeidaHandler> logger,
            IValidator<MarcarNotificacionLeidaCommand> validator)
        {
            _notificacionRepository = notificacionRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(MarcarNotificacionLeidaCommand request, CancellationToken cancellationToken)
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

                var notificacion = await _notificacionRepository.GetByIdAsync(request.NotificacionId);
                if (notificacion == null)
                {
                    result.Success = false;
                    result.Message = "Notificación no encontrada.";
                    return result;
                }

                notificacion.Leida = true;
                _notificacionRepository.Update(notificacion);
                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = true;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al marcar la notificación como leída.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al marcar la notificación como leída.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado.";
                return result;
            }
        }
    }
}
