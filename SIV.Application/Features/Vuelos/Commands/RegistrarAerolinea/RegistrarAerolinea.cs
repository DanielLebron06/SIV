using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Auditoria;
using SIV.Application.Common.Extensions;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Aerolinea;
using SIV.Domain.Emuns;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Vuelos.Commands.RegistrarAerolinea
{
    /// <summary>
    /// Comando para registrar una aerolínea.
    /// </summary>
    public class RegistrarAerolineaCommand : IRequest<Result<bool>>
    {
        public RegistroAerolineaDTO Datos { get; set; } = new();
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para RegistrarAerolineaCommand.
    /// </summary>
    public class RegistrarAerolineaValidator : AbstractValidator<RegistrarAerolineaCommand>
    {
        public RegistrarAerolineaValidator()
        {
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("Se requiere el ID del usuario.");
            RuleFor(x => x.Datos).NotNull().WithMessage("Los datos son requeridos.");
            RuleFor(x => x.Datos.CodigoIATA).Requerido("Código IATA").Length(2).WithMessage("El código IATA de una aerolínea debe tener exactamente 2 caracteres.")
                .Matches("^[A-Za-z]{2}$").WithMessage("El código IATA debe contener solo letras.");
            RuleFor(x => x.Datos.Nombre).Requerido("Nombre");
        }
    }

    /// <summary>
    /// Manejador para RegistrarAerolineaCommand.
    /// </summary>
    public class RegistrarAerolineaHandler : IRequestHandler<RegistrarAerolineaCommand, Result<bool>>
    {
        private readonly IAerolineaRepository _aerolineaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegistrarAerolineaHandler> _logger;
        private readonly IValidator<RegistrarAerolineaCommand> _validator;

        public RegistrarAerolineaHandler(
            IAerolineaRepository aerolineaRepository,
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<RegistrarAerolineaHandler> logger,
            IValidator<RegistrarAerolineaCommand> validator)
        {
            _aerolineaRepository = aerolineaRepository;
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(RegistrarAerolineaCommand request, CancellationToken cancellationToken)
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
                if (usuario == null || !usuario.EsAdministrador())
                {
                    result.Success = false;
                    result.Message = "Solo un administrador puede realizar esta acción.";
                    return result;
                }

                var existe = await _aerolineaRepository.ExistePorCodigoAsync(request.Datos.CodigoIATA);
                if (existe)
                {
                    result.Success = false;
                    result.Message = "Ya existe una aerolínea con ese código.";
                    return result;
                }

                var aerolinea = new Aerolinea
                {
                    Nombre = request.Datos.Nombre,
                    CodigoIATA = request.Datos.CodigoIATA,
                    Activo = true
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

                result.Success = true;
                result.Data = true;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al registrar aerolínea.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al registrar la aerolínea.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar aerolínea.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al registrar la aerolínea.";
                return result;
            }
        }
    }
}
