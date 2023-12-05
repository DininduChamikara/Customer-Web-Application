using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TokenAuthDemo2.Models;

namespace TokenAuthDemo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
      /*  private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuation) 
        {
            _configuration = configuation;
        }*/

      /*  [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] User user)
        {
            if (user.Username.Equals("admin") && user.Password.Equals("password"))
            {
                var token = GenerateJwtToken(user);
                return Ok(token);
            }
            return BadRequest("Invalid user");
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

            var claims = new Claim[] {
                    new Claim(ClaimTypes.Name,user.Username)
                };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/
    }
}
