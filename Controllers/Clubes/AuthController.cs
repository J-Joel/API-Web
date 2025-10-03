using API_Web.BDD;
using API_Web.Models.Clubes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Web.Controllers.Clubes
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Clubes")]
    public class AuthController : ControllerBase
    {
        public IConfiguration _config;
        public AuthController(IConfiguration config) 
        {
            _config = config;
        }

        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest req)
        {
            UsuarioDTO usuario = BDDConexion.usuarioTabla.UsuarioPorUsuNomb(req.Username);
            if (usuario == null)
                return Unauthorized(new { message = "aCredenciales invalidas" });
            if (!BDDConexion.usuarioTabla.ContraseñaHashValidado(usuario.UsuarioId, req.Password))
                return Unauthorized(new { message = "bCredenciales invalidas" });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.UsuarioNombre),
                new Claim(ClaimTypes.Role, usuario.Rol),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);


            return Ok(new LoginResponse { Token = jwt, ExpiresAtUtc = token.ValidTo });
        }
    }
}
