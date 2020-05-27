using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cw3.DAL;
using Cw3.DTOs.Requests;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cw3.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IDbAuthService _service;
        private IConfiguration Configuration;
        public AuthController(IDbAuthService service, IConfiguration config)
        {
            _service = service;
            Configuration = config;
        }
        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            String saltString = _service.GetSalt(request.Login);
            if (saltString == null)
                return Unauthorized("Incorrect username.");
            var salt = Convert.FromBase64String(saltString);
            string hashedPw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: request.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8
            ));
            String role = _service.AuthenticateAndGetRole(request.Login, hashedPw);
            if (role == null)
                return Unauthorized("Incorrect password.");
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, request.Login),
                new Claim(ClaimTypes.Role, role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );
            return Ok( new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()
            });
        }
    }
}