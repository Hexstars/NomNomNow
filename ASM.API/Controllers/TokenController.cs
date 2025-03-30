using ASM.Share.Models.Requests;
using ASM.Share.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IAccountSvc _accountSvc;
        public IConfiguration _configuration;
        public TokenController(IAccountSvc accountSvc, IConfiguration configuration)
        {
            _accountSvc = accountSvc;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            var account = _accountSvc.Login(request);
            if (account == null)
            {
                return Unauthorized("Thông tin đăng nhập không đúng");
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),

                new Claim("Id", account.AccountId.ToString()),
                new Claim("Email", account.Email),

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signIn);
            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
