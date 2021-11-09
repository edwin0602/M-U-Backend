using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestBackend.Api.Wrappers;
using RestBackend.Core.Resources;
using RestBackend.Core.Services;
using System.Threading.Tasks;

namespace RestBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Create an User
        /// </summary>
        /// <response code="200">Created user message</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [AllowAnonymous]
        [HttpPost("Register")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register(CreateUserResource userResource)
        {
            var tokenResponse = await _userService.Create(userResource);
            if (tokenResponse != default)
                return Ok(new Response<string>($"User {userResource.UserName} created."));

            return BadRequest(Response<string>.BadResponse("Cannot register!"));
        }

        /// <summary>
        /// Generate a login token
        /// </summary>
        /// <response code="200">Token</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        [ProducesResponseType(typeof(Response<TokenResource>), 200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Authenticate(LoginResource userLoginResource)
        {
            var tokenResponse = await _userService.Authenticate(userLoginResource);
            if (tokenResponse != default)
                return Ok(new Response<TokenResource>(tokenResponse));

            return BadRequest(Response<string>.BadResponse("Email or password incorrect."));
        }

        /// <summary>
        /// Send a forgot password token
        /// </summary>
        /// <response code="200">Forgot password token sended</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [AllowAnonymous]
        [HttpPost("{userName}/ForgotPassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GenerateForgotPasswordToken(string userName, string returnUrl = null)
        {
            await _userService.SendForgotPasswordToken(userName, returnUrl);
            return Ok();
        }

        /// <summary>
        /// Update password using a forgot password token 
        /// </summary>
        /// <response code="200">Password was updated</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [AllowAnonymous]
        [HttpPut("{userId}/ResetPassword")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ResetPassword(string userId, [FromBody] ResetPasswordResource resetPasswordResource)
        {
            await _userService.ResetPassword(userId, resetPasswordResource);

            return Ok(new Response<string>
            {
                Message = "Password was updated.",
                Succeeded = true
            });
        }

    }
}
