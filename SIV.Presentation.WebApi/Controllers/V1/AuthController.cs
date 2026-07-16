using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SIV.Application.DTOs.Usuario;
using SIV.Application.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _config;

    public AuthController(IUserService userService, IConfiguration config)
    {
        _userService = userService;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO login)
    {
        try
        {
            var user = await _userService.InicioSesion(login);
            var token = GenerarTokenJWT(user);
            return Ok(new { token });
        }
        catch (Exception)
        {
            return Unauthorized(new { mensaje = "Credenciales incorrectas" });
        }
    }
    private string GenerarTokenJWT(UsuarioDTO usuario)
    {
        // 1. Obtenemos la clave secreta desde el appsettings.json
        var secretKey = _config["Jwt:Key"];
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);

        // 2. Definimos los Claims (la información que llevará el token)
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.Email),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Role, usuario.Rol.ToString())
        };

        // 3. Creamos las credenciales de firma
        var creds = new SigningCredentials(
            new SymmetricSecurityKey(keyBytes),
            SecurityAlgorithms.HmacSha256
        );

        // 4. Creamos el objeto del Token
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2), // El token expira en 2 horas
            signingCredentials: creds
        );

        // 5. Retornamos el token como string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}