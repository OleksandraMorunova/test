using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using restful.Auth.Config;
using restful.Auth.Service;

namespace restful.Auth.Controller
{
    [ApiController]
    [Route("auth")]
    public class HomeController : ControllerBase
    { 
        private readonly IAuthService _service;

        public HomeController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var token = await _service.GetToken(model);
            if(token != null)
            {
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    message = "Succesfully"
                });
            }                       
            return Unauthorized();
        }

        [HttpPost("/create/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> СreateRole(RoleRequest request)
        {
            var result = await _service.СreateRole(request);
            if (result.Equals(true))
            {
                return Ok(new
                {
                    message = "Succesfully"
                });
            }
            return BadRequest(new 
            {
                message = "Unsuccesfully"
            });
        }
    }
}