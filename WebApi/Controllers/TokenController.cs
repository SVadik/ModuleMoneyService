using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.DTO;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private IUserService _service;
        private readonly AuthOptions _authOptions;

        public TokenController(IUserService service, IOptions<AuthOptions> authOptionsAccessor)
        {
            _service = service;
            _authOptions = authOptionsAccessor.Value;
        }

        [HttpPost]
        public IActionResult Get([FromBody] UserCredentials user)
        {
            if(_service.IsValidUser(user.Username, user.Password))
            {
                var dbUser = _service.GetByUsername(user.Username);
                var authClaims = new[]
                {
                    new Claim(ClaimTypes.Name, dbUser.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var token = new JwtSecurityToken(
                    issuer: _authOptions.Issuer,
                    audience: _authOptions.Audience,
                    expires: DateTime.Now.AddMinutes(_authOptions.ExpiresInMinutes),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecureKey)),
                        SecurityAlgorithms.HmacSha256Signature)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    success = true,
                    message = dbUser.Firstname
                });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterUserModel model)
        {
            var user = _service.Create(new User(model.Username, model.Firstname, model.Password));

            if (user == null)
                return BadRequest(new { message = "Username is already taken" });

            return Ok(new {
                success = true,
                message = user.Firstname
            });
        }
    }
}