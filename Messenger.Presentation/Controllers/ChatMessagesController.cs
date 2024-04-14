using Asp.Versioning;
using Messenger.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.ChatDto;
using Shared.DataTransferObjects.ChatMessageDto;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Messenger.Presentation.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiversion}/chats/{chatId:guid}/messages")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1.0")]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ChatMessagesController(IServiceManager service) => _service = service;


        /// <summary>
        /// Returns messages of a chat
        /// </summary>
        /// <param name="messageParameters">Request parameters</param>
        /// <param name="chatId">The chat id</param>
        /// <response code="200">Returns all messages of the chat</response>
        /// <response code="404">If the chat doesn't exist in the database</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetChatMessages(Guid chatId, [FromQuery] ChatMessageParameters messageParameters)
        {
            var pagedResult = await _service.ChatService.GetChatMessagesAsync(chatId, messageParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.chatMessages);
        }

        /// <summary>
        /// Returns a message by id
        /// </summary>
        /// <param name="messageId">The message id</param>
        /// <response code="200">Returns the message</response>
        /// <response code="404">If the message doesn't exist in the database</response>
        [HttpGet("{messageId:guid}", Name = "ChatMessageById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetChatMessage(Guid messageId)
        {
            var chat = await _service.ChatService.GetChatMessageAsync(messageId, trackChanges: false);
            return Ok(chat);
        }

        /// <summary>
        /// Creates a message for a chat
        /// </summary>
        /// <param name="chatId">The chat id</param>
        /// <param name="chatMessage">The message data for creation</param>
        /// <response code="201">Returns the newly created message</response>
        /// <response code="400">If the request body is null</response>
        /// <response code="404">If the chat doesn't exist in the database</response>
        /// <response code="422">If the message data for creation is invalid</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateChatMessage(Guid chatId, [FromBody] ChatMessageForCreationDto chatMessage)
        {
            var chatMessageToReturn = await _service.ChatService.CreateChatMessageAsync(chatId, chatMessage, trackChanges: false);

            return CreatedAtRoute("ChatMessageById", new
            {
                chatId,
                messageId = chatMessageToReturn.Id
            },
           chatMessageToReturn);

        }

        /// <summary>
        /// Deletes a message
        /// </summary>
        /// <param name="chatMessageId">The message id</param>
        /// <response code="204">Successful removal</response>
        /// <response code="404">If the message doesn't exist in the database</response>
        [HttpDelete("{chatMessageId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteChatMessage(Guid chatMessageId)
        {
            await _service.ChatService.DeleteChatMessageAsync(chatMessageId, trackChanges: false);
            return NoContent();
        }
    }
}
