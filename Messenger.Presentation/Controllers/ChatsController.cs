using Asp.Versioning;
using Messenger.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.ChatDto;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Messenger.Presentation.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiversion}/chats")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1.0")]
    public class ChatsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ChatsController(IServiceManager service) => _service = service;

        /// <summary>
        /// Returns chats
        /// </summary>
        /// <param name="chatParameters">Request parameters</param>
        /// <response code="200">Returns all chats</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetChats([FromQuery] ChatParameters chatParameters)
        {
            var pagedResult = await _service.ChatService.GetChatsAsync(chatParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.chats);
        }

        /// <summary>
        /// Returns all chats of an account
        /// </summary>
        /// <param name="chatsParameters">Request parameters</param>
        /// <param name="accountId">The account id</param>
        /// <response code="200">Returns all chats of the account</response>
        /// <response code="404">If the account doesn't exist in the database</response>
        [HttpGet("account/{accountId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAccountChats(Guid accountId, [FromQuery] ChatParameters chatsParameters)
        {
            var pagedResult = await _service.ChatService.GetAccountChatsAsync(accountId, chatsParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.chats);
        }

        /// <summary>
        /// Returns a chat by id
        /// </summary>
        /// <param name="chatId">The chat id</param>
        /// <response code="200">Returns the chat</response>
        /// <response code="404">If the chat doesn't exist in the database</response>
        [HttpGet("{chatId:guid}", Name = "ChatById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetChat(Guid chatId)
        {
            var chat = await _service.ChatService.GetChatAsync(chatId, trackChanges: false);
            return Ok(chat);
        }


        /// <summary>
        /// Creates a chat
        /// </summary>
        /// <param name="accountId">The account id</param>
        /// <param name="chat">The chat data for creation</param>
        /// <response code="201">Returns the newly created chat</response>
        /// <response code="400">If the request body is null</response>
        /// <response code="404">If the account doesn't exist in the database</response>
        /// <response code="422">If the chat data for creation is invalid</response>
        [HttpPost("account/{accountId:guid}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateChat(Guid accountId,  ChatForCreationDto chat)
        {
            var chatToReturn = await _service.ChatService.CreateChatAsync(accountId, chat, trackChanges: false);

            return CreatedAtRoute("ChatById", new
            {
                accountId,
                chatId = chatToReturn.Id
            },
           chatToReturn);

        }


        /// <summary>
        /// Adds a member to a chat 
        /// </summary>
        /// <param name="accountId">The account id</param>
        /// <param name="chatId">The chat id</param>
        /// <response code="200">The member added successfully</response>
        /// <response code="404">If the account doesn't exist in the database or if the chat doesn't exist in the database</response>
        [HttpPost("{chatId:guid}/account/{accountId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddMemberToChat(Guid accountId, Guid chatId)
        {
            await _service.ChatService.AddMemberToChatAsync(accountId, chatId, chatTrackChanges: true, accountTrackChanges: false);

            return Ok();
        }

        /// <summary>
        /// Deletes a chat by id
        /// </summary>
        /// <param name="chatId">The chat id</param>
        /// <response code="204">Deleted the chat successfully</response>
        /// <response code="404">If the chat doesn't exist in the database</response>
        [HttpDelete("{chatId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteChat(Guid chatId)
        {
            await _service.ChatService.DeleteChatAsync(chatId, trackChanges: false);
            return NoContent();
        }

        /// <summary>
        /// Removes a member from a chat 
        /// </summary>
        /// <param name="chatId">The chat id</param>
        /// <param name="accountId">The acoount id</param>
        /// <response code="204">Removed successfully</response>
        /// <response code="404">If the account doesn't exist in the database or if the chat doesn't exist in the database</response>
        [HttpDelete("exit/{chatId:guid}/account/{accountId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> LeaveChat(Guid accountId, Guid chatId)
        {
            await _service.ChatService.LeaveChatAsync(accountId, chatId, trackChanges: false);
            return NoContent();
        }

        /// <summary>
        /// Updates a chat
        /// </summary>
        /// <param name="chatId">The chat id</param>
        /// <param name="chat">The chat data for update</param>
        /// <response code="204">Updated successfully</response>
        /// <response code="400">If the request body is null</response>
        /// <response code="404">If the chat doesn't exist in the database</response>
        /// <response code="422">If the chat data for update is invalid</response>
        [HttpPut("{chatId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateChat( Guid chatId, [FromBody] ChatForUpdateDto chat)
        {
            await _service.ChatService.UpdateChatAsync(chatId, chat, chatTrackChanges: true, accountTrackChanges: false);
            return NoContent();
        }
    }
}
