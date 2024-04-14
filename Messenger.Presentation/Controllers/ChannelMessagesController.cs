using Asp.Versioning;
using Messenger.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.ChannelMessageDto;
using Shared.DataTransferObjects.ChatMessageDto;
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
    [Route("api/v{v:apiversion}/channels/{channelId}/messages")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1.0")]
    public class ChannelMessagesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ChannelMessagesController(IServiceManager service) => _service = service;

        /// <summary>
        /// Returns messages of a channel
        /// </summary>
        /// <param name="messageParameters">Request parameters</param>
        /// <param name="channelId">The channel id</param>
        /// <response code="200">Returns all messages of the channel</response>
        /// <response code="404">If the channel doesn't exist in the database</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetChannelMessages(Guid channelId, [FromQuery] ChannelMessageParameters messageParameters)
        {
            var pagedResult = await _service.ChannelService.GetChannelMessagesAsync(channelId, messageParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.channelMessages);
        }

        /// <summary>
        /// Returns a message by id
        /// </summary>
        /// <param name="messageId">The message id</param>
        /// <response code="200">Returns the message</response>
        /// <response code="404">If the message doesn't exist in the database</response>
        [HttpGet("{messageId:guid}", Name = "ChannelMessageById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetChatMessage(Guid messageId)
        {
            var channel = await _service.ChannelService.GetChannelMessageAsync(messageId, trackChanges: false);
            return Ok(channel);
        }

        /// <summary>
        /// Creates a message for a channel
        /// </summary>
        /// <param name="channelId">The channel id</param>
        /// <param name="channelMessage">The message data for creation</param>
        /// <response code="201">Returns the newly created message</response>
        /// <response code="400">If the request body is null</response>
        /// <response code="404">If the channel doesn't exist in the database</response>
        /// <response code="422">If the message data for creation is invalid</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateChannelMessage(Guid channelId, [FromBody] ChannelMessageForCreationDto channelMessage)
        {
            var channelMessageToReturn = await _service.ChannelService.CreateChannelMessageAsync(channelId, channelMessage, trackChanges: false);

            return CreatedAtRoute("ChannelMessageById", new
            {
                channelId,
                messageId = channelMessageToReturn.Id
            },
           channelMessageToReturn);

        }

        /// <summary>
        /// Deletes a message
        /// </summary>
        /// <param name="channelMessageId">The message id</param>
        /// <response code="204">Successful removal</response>
        /// <response code="404">If the message doesn't exist in the database</response>
        [HttpDelete("{channelMessageId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteChannelMessage(Guid channelMessageId)
        {
            await _service.ChannelService.DeleteChannelMessageAsync(channelMessageId, trackChanges: false);
            return NoContent();
        }
    }
}
