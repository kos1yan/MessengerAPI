using Asp.Versioning;
using Messenger.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Presentation.Controllers
{

    [ApiVersion("1.0")] 
    [Route("api/v{v:apiversion}/authentication")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1.0")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;
        public AuthenticationController(IServiceManager service) => _service = service;



        /// <summary>
        /// Registers an user
        /// </summary>
        /// <param name="userForRegistration">The user data for registration</param>
        /// <response code="201">Successfull registration</response>
        /// <response code="400">If the request body is null</response>
        /// <response code="422">If the user data is invalid</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _service.AuthenticationService.RegisterUser(userForRegistration);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }


        /// <summary>
        /// Authenticates an user
        /// </summary>
        /// <param name="user">The user data for authentication</param>
        /// <returns>A newly created access token and a refresh token</returns>
        /// <response code="200">Returns an access token and a refresh token</response>
        /// <response code="400">If the request body is null</response>
        /// <response code="422">If the user data is invalid</response>
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _service.AuthenticationService.ValidateUser(user)) return Unauthorized();
            var tokenDto = await _service.AuthenticationService.CreateToken(populateExp: true);

            return Ok(tokenDto);
        }
    }
}
