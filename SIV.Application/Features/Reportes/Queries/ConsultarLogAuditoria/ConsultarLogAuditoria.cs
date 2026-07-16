using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Auditoria;
using SIV.Application.Common.Extensions;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Auditoria;
using SIV.Domain.Emuns;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SIV.Domain.Entities;

namespace SIV.Application.Features.Reportes.Queries.ConsultarLogAuditoria
{
    /// <summary>
    /// Consulta para buscar el log de auditoría.
    /// </summary>
    public class ConsultarLogAuditoriaQuery : IRequest<Result<List<LogAuditoriaDTO>>>
    {
        public FiltroAuditoriaDTO Filtros { get; set; } = new();
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para ConsultarLogAuditoriaQuery.
    /// </summary>
    public class ConsultarLogAuditoriaValidator : AbstractValidator<ConsultarLogAuditoriaQuery>
    {
        public ConsultarLogAuditoriaValidator()
        {
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("El ID del usuario es requerido.");
            RuleFor(x => x.Filtros).NotNull().WithMessage("Los filtros no pueden ser nulos.");
        }
    }

    /// <summary>
    /// Manejador para ConsultarLogAuditoriaQuery.
    /// </summary>
    public class ConsultarLogAuditoriaHandler : IRequestHandler<ConsultarLogAuditoriaQuery, Result<List<LogAuditoriaDTO>>>
    {
        private readonly ILogAuditoriaRepository _logAuditoriaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ConsultarLogAuditoriaHandler> _logger;
        private readonly IValidator<ConsultarLogAuditoriaQuery> _validator;

        public ConsultarLogAuditoriaHandler(
            ILogAuditoriaRepository logAuditoriaRepository,
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<ConsultarLogAuditoriaHandler> logger,
            IValidator<ConsultarLogAuditoriaQuery> validator)
        {
            _logAuditoriaRepository = logAuditoriaRepository;
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<List<LogAuditoriaDTO>>> Handle(ConsultarLogAuditoriaQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<List<LogAuditoriaDTO>>();

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
                if (usuario == null || !usuario.EsAuditorOAdministrador())
                {
                    result.Success = false;
                    result.Message = "Solo administradores o auditores pueden acceder a la auditoría.";
                    return result;
                }

                var logs = (await _logAuditoriaRepository.BuscarConFiltrosAsync(
                    request.Filtros.Actor,
                    request.Filtros.Modulo,
                    request.Filtros.TipoAccion,
                    request.Filtros.FechaInicio,
                    request.Filtros.FechaFin
                )).ToList();

                List<LogAuditoriaDTO> resultado = new();

                foreach (var log in logs)
                {
                    resultado.Add(MapearLogAuditoria(log));
                }

                await _auditoriaManager.Registrar(
                    usuario.Email,
                    Modulo.Vuelos,
                    TipoAccion.Actualizar,
                    "Consulta de log de auditoría realizada",
                    null,
                    usuario.Email
                );

                await _unitOfWork.SaveChangesAsync();

                result.Success = true;
                result.Data = resultado;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al consultar la auditoría.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al consultar la auditoría.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al consultar la auditoría.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al consultar la auditoría.";
                return result;
            }
        }

        private LogAuditoriaDTO MapearLogAuditoria(LogAuditoria log)
        {
            return new LogAuditoriaDTO
            {
                Id = log.Id,
                Actor = log.Actor,
                Modulo = log.Modulo,
                TipoAccion = log.TipoAccion,
                Resultado = log.Resultado,
                EntidadAfectadaId = log.EntidadAfectadaId,
                EntidadAfectadaDescripcion = log.DescripcionEntidad,
                FechaHora = log.FechaHora
            };
        }
    }
}
