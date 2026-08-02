using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SIV.Application.DTOs.Usuario;
using SIV.Application.Features.Usuarios.Commands.IniciarSesion;
using SIV.Presentation.WebApi.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SIV.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IConfiguration _config;

        public AuthController(ISender sender, IConfiguration config)
        {
            _sender = sender;
            _config = config;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var result = await _sender.Send(new IniciarSesionCommand { Email = login.Email, Password = login.Password });
            if (!result.Success || result.Data == null) return Unauthorized(ApiResponse.Error(result.Message));

            var token = GenerarTokenJWT(result.Data);
            return Ok(new { token });
        }

        private string GenerarTokenJWT(UsuarioDTO usuario)
        {
            var secretKey = _config["Jwt:Key"] ?? string.Empty;
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString())
            };

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
