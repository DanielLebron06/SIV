    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using SIV.Application.Common.Models;
    using SIV.Application.DTOs.Usuario;
    using SIV.Domain.Repositories;
    using System.Data.Common;
    using FluentValidation;
    using MediatR;
    using SIV.Application.Common.Models;
    using SIV.Application.DTOs.Usuario;

    namespace SIV.Application.Features.Usuarios.Queries.ObtenerUsuariosInternos
    {
        public class ObtenerUsuariosInternosQuery : IRequest<Result<List<UsuarioInternoDTO>>>
        {
            public bool? Activo { get; set; }
        }

        public class ObtenerUsuariosInternosValidator
            : AbstractValidator<ObtenerUsuariosInternosQuery>
        {
            public ObtenerUsuariosInternosValidator()
            {

            }
        }
        public class ObtenerUsuariosInternosHandler
            : IRequestHandler<ObtenerUsuariosInternosQuery, Result<List<UsuarioInternoDTO>>>
        {
            private readonly IUsuarioRepository _usuarioRepository;

            private readonly ILogger<ObtenerUsuariosInternosHandler> _logger;

            private readonly IValidator<ObtenerUsuariosInternosQuery> _validator;

            public ObtenerUsuariosInternosHandler(
                IUsuarioRepository usuarioRepository,
                ILogger<ObtenerUsuariosInternosHandler> logger,
                IValidator<ObtenerUsuariosInternosQuery> validator)
            {
                _usuarioRepository = usuarioRepository;
                _logger = logger;
                _validator = validator;
            }

            public async Task<Result<List<UsuarioInternoDTO>>> Handle(
                ObtenerUsuariosInternosQuery request,
                CancellationToken cancellationToken)
            {
                var result = new Result<List<UsuarioInternoDTO>>();

                try
                {
                    var validation = await _validator.ValidateAsync(request, cancellationToken);

                    if (!validation.IsValid)
                    {
                        result.Success = false;

                        result.Message =
                            "Errores de validación: " +
                            string.Join(", ",
                            validation.Errors.Select(x => x.ErrorMessage));

                        return result;
                    }

                    var usuarios =
                        await _usuarioRepository.BuscarInternos(request.Activo);

                    var listado = usuarios
                        .Select(x => new UsuarioInternoDTO
                        {
                            Id = x.Id,
                            Email = x.Email,
                            Rol = x.Rol,
                            Activo = x.Activo
                        })
                        .ToList();

                    result.Success = true;
                    result.Data = listado;

                    return result;
                }
                catch (DbException ex)
                {
                    _logger.LogError(
                        ex,
                        "Error de base de datos al consultar usuarios internos.");

                    result.Success = false;
                    result.Message =
                        "Ocurrió un error en la base de datos al consultar los usuarios internos.";

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error inesperado al consultar usuarios internos.");

                    result.Success = false;
                    result.Message =
                        "Ocurrió un error inesperado al consultar los usuarios internos.";

                    return result;
                }
            }
        }
    }
