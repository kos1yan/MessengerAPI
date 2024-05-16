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
    [Route("api/v{v:apiversion}/token")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1.0")]
    public class TokenController : ControllerBase
    {
        private readonly IServiceManager _service;
        public TokenController(IServiceManager service) => _service = service;

        /// <summary>
        /// Generates a new access token and refresh token
        /// </summary>
        /// <param name="refreshToken">The valid refresh token</param>
        /// <returns>A newly created access token and a refresh token</returns>
        /// <response code="200">Returns an access token and a refresh token</response>
        /// <response code="400">If the header is null or if the token has some invalid values.</response>
        [HttpPost("refresh")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Refresh([FromHeader(Name = "Refresh-Token")] string refreshToken)
        {
            var tokenDtoToReturn = await _service.AuthenticationService.RefreshToken(refreshToken);
            return Ok(tokenDtoToReturn);
        }
    }
}
