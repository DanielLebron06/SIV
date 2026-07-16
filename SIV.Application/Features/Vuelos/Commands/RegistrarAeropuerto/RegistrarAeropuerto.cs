using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Auditoria;
using SIV.Application.Common.Extensions;
using SIV.Application.Common.Models;
using SIV.Application.DTOs.Aeropuerto;
using SIV.Domain.Emuns;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;
using SIV.Domain.Repositories;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Features.Vuelos.Commands.RegistrarAeropuerto
{
    /// <summary>
    /// Comando para registrar un aeropuerto.
    /// </summary>
    public class RegistrarAeropuertoCommand : IRequest<Result<bool>>
    {
        public RegistroAeropuertoDTO Datos { get; set; } = new();
        public Guid EjecutadorId { get; set; }
    }

    /// <summary>
    /// Validador para RegistrarAeropuertoCommand.
    /// </summary>
    public class RegistrarAeropuertoValidator : AbstractValidator<RegistrarAeropuertoCommand>
    {
        public RegistrarAeropuertoValidator()
        {
            RuleFor(x => x.EjecutadorId).NotEmpty().WithMessage("Se requiere el ID del usuario.");
            RuleFor(x => x.Datos).NotNull().WithMessage("Los datos son requeridos.");
            RuleFor(x => x.Datos.CodigoIATA).Requerido("Código IATA").Length(3).WithMessage("El código IATA debe tener 3 caracteres.");
            RuleFor(x => x.Datos.Nombre).Requerido("Nombre");
            RuleFor(x => x.Datos.Ciudad).Requerido("Ciudad");
            RuleFor(x => x.Datos.Pais).Requerido("País");
        }
    }

    /// <summary>
    /// Manejador para RegistrarAeropuertoCommand.
    /// </summary>
    public class RegistrarAeropuertoHandler : IRequestHandler<RegistrarAeropuertoCommand, Result<bool>>
    {
        private readonly IAeropuertoRepository _aeropuertoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuditoriaManager _auditoriaManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegistrarAeropuertoHandler> _logger;
        private readonly IValidator<RegistrarAeropuertoCommand> _validator;

        public RegistrarAeropuertoHandler(
            IAeropuertoRepository aeropuertoRepository,
            IUsuarioRepository usuarioRepository,
            IAuditoriaManager auditoriaManager,
            IUnitOfWork unitOfWork,
            ILogger<RegistrarAeropuertoHandler> logger,
            IValidator<RegistrarAeropuertoCommand> validator)
        {
            _aeropuertoRepository = aeropuertoRepository;
            _usuarioRepository = usuarioRepository;
            _auditoriaManager = auditoriaManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<bool>> Handle(RegistrarAeropuertoCommand request, CancellationToken cancellationToken)
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

                var existe = await _aeropuertoRepository.BuscarPorCodigoAsync(request.Datos.CodigoIATA);
                if (existe != null)
                {
                    result.Success = false;
                    result.Message = "Ya existe un aeropuerto con ese código.";
                    return result;
                }

                var aeropuerto = new Aeropuerto
                {
                    Nombre = request.Datos.Nombre,
                    CodigoIATA = request.Datos.CodigoIATA,
                    Ciudad = request.Datos.Ciudad,
                    Pais = request.Datos.Pais,
                    Activo = true
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

                result.Success = true;
                result.Data = true;
                return result;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error de base de datos al registrar aeropuerto.");
                result.Success = false;
                result.Message = "Ocurrió un error en la base de datos al registrar el aeropuerto.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar aeropuerto.");
                result.Success = false;
                result.Message = "Ocurrió un error inesperado al registrar el aeropuerto.";
                return result;
            }
        }
    }
}
