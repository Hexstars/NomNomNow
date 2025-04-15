using ASM.Share.Models;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var account = await _accountSvc.Login(request);
            if (account == null)
            {
                return Unauthorized("Thông tin đăng nhập không đúng.");
            }
            var claims = new[]
            {
                //new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role.ToString())

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(Account request)
        {
            var account = new Account
            {
                Email = request.Email,
                Password = request.Password,
                FullName = request.FullName,
                Phone = request.Phone,
                Address = request.Address,
                Role = Role.User
            };
            var result = await _accountSvc.Register(account);
            if (result)
            {
                return Ok("Đăng ký thành công.");
            }
            return BadRequest("Email đã tồn tại.");
        }
    }
}
