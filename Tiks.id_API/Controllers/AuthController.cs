using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tiks.id_API.Models;

namespace Tiks.id_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        TiksIdContext ctx = new TiksIdContext();

        [HttpPost("Login")]
        public IActionResult Login(Auth authData)
        {
            var user = ctx.Users.FirstOrDefault(s => s.Email == authData.email);
            if (user == null) return NotFound();
            if (user.Password != authData.password) return BadRequest();

            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Email, authData.email),
            };
            var token = new JwtSecurityToken
                (
                "http://localhost",
                "http://localhost",
                claim,
                expires: DateTime.Now.AddHours(4),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("hsdkjahdjkhshahkhshsakhkahkhskhhdahkdhskhkahkdhskjhdkshd")), SecurityAlgorithms.HmacSha256)
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiredAt = DateTime.Now.AddHours(4),
            });
        }
    }
}
