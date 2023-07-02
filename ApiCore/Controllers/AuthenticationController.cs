using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiCore.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ApiCore.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly string _secretkey;

    public AuthenticationController(IConfiguration config)
    {
        _secretkey = config.GetSection("settings").GetSection("secretkey").ToString()!;
    }

    [HttpPost]
    [Route("validate")]
    public IActionResult Validate([FromBody] User user)
    {
        if (user is not null && user.Email == "c@mail.com" && user.Password == "123") 
        {
            var keyBytes = Encoding.ASCII.GetBytes(_secretkey);
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Email));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string token = tokenHandler.WriteToken(tokenConfig);

            return StatusCode(StatusCodes.Status200OK, new { Token = token });
        }
        else
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { Token = "" });
        }
    }
}
