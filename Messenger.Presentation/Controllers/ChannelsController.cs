using Asp.Versioning;
using Messenger.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.ChannelDto;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Messenger.Presentation.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiversion}/channels")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1.0")]
    public class ChannelsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ChannelsController(IServiceManager service) => _service = service;


        /// <summary>
        /// Returns channels
        /// </summary>
        /// <param name="channelParameters">Request parameters</param>
        /// <response code="200">Returns all channels</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetChannels([FromQuery] ChannelParameters channelParameters)
        {
            var pagedResult = await _service.ChannelService.GetChannelsAsync(channelParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.channels);
        }

        /// <summary>
        /// Returns all channels of an account
        /// </summary>
        /// <param name="channelsParameters">Request parameters</param>
        /// <param name="accountId">The account id</param>
        /// <response code="200">Returns all channels of the account</response>
        /// <response code="404">If the account doesn't exist in the database</response>
        [HttpGet("account/{accountId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAccountChannels(Guid accountId, [FromQuery] ChannelParameters channelsParameters)
        {
            var pagedResult = await _service.ChannelService.GetAccountChannelsAsync(accountId, channelsParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.channels);
        }

        /// <summary>
        /// Returns a channel by id
        /// </summary>
        /// <param name="channelId">The channel id</param>
        /// <response code="200">Returns the channel</response>
        /// <response code="404">If the channel doesn't exist in the database</response>
        [HttpGet("{channelId:guid}", Name = "ChannelById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetChannel(Guid channelId)
        {
            var channel = await _service.ChannelService.GetChannelAsync(channelId, trackChanges: false);
            return Ok(channel);
        }

        /// <summary>
        /// Creates a channel
        /// </summary>
        /// <param name="accountId">The account id</param>
        /// <param name="channel">The channel data for creation</param>
        /// <response code="201">Returns the newly created channel</response>
        /// <response code="400">If the request body is null</response>
        /// <response code="404">If the account doesn't exist in the database</response>
        /// <response code="422">If the channel data for creation is invalid</response>
        [HttpPost("account/{accountId:guid}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateChannel(Guid accountId, [FromBody] ChannelForCreationDto channel)
        {
            var channelToReturn = await _service.ChannelService.CreateChannelAsync(accountId, channel, trackChanges: false);

            return CreatedAtRoute("ChannelById", new
            {
                accountId,
                channelId = channelToReturn.Id
            },
           channelToReturn);

        }

        /// <summary>
        /// Adds a member to a channel 
        /// </summary>
        /// <param name="accountId">The account id</param>
        /// <param name="channelId">The channel id</param>
        /// <response code="200">The member added successfully</response>
        /// <response code="404">If the account doesn't exist in the database or if the channel doesn't exist in the database</response>
        [HttpPost("{channelId:guid}/account/{accountId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddMemberToChannel(Guid accountId, Guid channelId)
        {
            await _service.ChannelService.AddMemberToChannelAsync(accountId, channelId, channelTrackChanges: true, accountTrackChanges: false);

            return Ok();
        }

        /// <summary>
        /// Deletes a channel by id
        /// </summary>
        /// <param name="channelId">The channel id</param>
        /// <response code="204">Deleted the channel successfully</response>
        /// <response code="404">If the channel doesn't exist in the database</response>
        [HttpDelete("{channelId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteChannel(Guid channelId)
        {
            await _service.ChannelService.DeleteChannelAsync(channelId, trackChanges: false);
            return NoContent();
        }

        /// <summary>
        /// Removes a member from a channel 
        /// </summary>
        /// <param name="channelId">The channel id</param>
        /// <param name="accountId">The account id</param>
        /// <response code="204">Removed successfully</response>
        /// <response code="404">If the account doesn't exist in the database or if the channel doesn't exist in the database</response>
        [HttpDelete("exit/{channelId:guid}/account/{accountId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> LeaveChannel(Guid accountId, Guid channelId)
        {
            await _service.ChannelService.LeaveChannelAsync(accountId, channelId, trackChanges: false);
            return NoContent();
        }

        /// <summary>
        /// Updates a channel
        /// </summary>
        /// <param name="channelId">The channel id</param>
        /// <param name="channel">The channel data for update</param>
        /// <response code="204">Updated successfully</response>
        /// <response code="400">If the request body is null</response>
        /// <response code="404">If the channel doesn't exist in the database</response>
        /// <response code="422">If the channel data for update is invalid</response>
        [HttpPut("{channelId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateChannel(Guid channelId, [FromBody] ChannelForUpdateDto channel)
        {
            await _service.ChannelService.UpdateChannelAsync(channelId, channel, channelTrackChanges: true, accountTrackChanges: false);
            return NoContent();
        }
    }
}
