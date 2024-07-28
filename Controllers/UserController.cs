using IdentityWebApiSample.Server.Entities;
using IdentityWebApiSample.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityWebApiSample.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly UserManager<UserSystem> _userSystem;

        public UserController(IAuthenticationService authenticationService, UserManager<UserSystem> userSystem)
        {
            _authenticationService = authenticationService;
            _userSystem = userSystem;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var response = await _authenticationService.Login(loginRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if necessary
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                var response = await _authenticationService.Register(registerRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if necessary
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
