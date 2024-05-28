using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Shared.DataTransferObjects.ChatMessageDto;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Messenger.Presentation.Hubs
{
    [Authorize]
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IServiceManager _service;
        public ChatHub(IServiceManager service)
        {
            _service = service;
        }
       
        public async Task SendMessageToChat(ChatHubMessageParameters message)
        {
            await Clients.Group(message.ConnectionId.ToString()).ReceiveMessage(message.AccountId, message.Text, message.Image, message.ChatId, message.Id);
        }

        public async Task DeleteChatMessage(ChatHubMessageParameters message)
        {
            await Clients.Group(message.ConnectionId.ToString()).DeleteNotification(message.ChatId, message.Id);
        }

        public async Task EditChatMessage(ChatHubMessageParameters message)
        {
            await Clients.Group(message.ConnectionId.ToString()).EditNotification(message.AccountId, message.ChatId, message.Id, message.Text, message.Image);
        }

        public async Task AddToChat(ChatHubGroupsParameters parameters)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, parameters.ConnectionId.ToString());

            await Clients.Group(parameters.ConnectionId.ToString()).ReceiveMessage($"{parameters.AccountName} has joined the group.");

        }

        public async Task RemoveFromChat(ChatHubGroupsParameters parameters)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, parameters.ConnectionId.ToString());

            await Clients.Group(parameters.ConnectionId.ToString()).ReceiveMessage($"{parameters.AccountName} has left the group.");
        }

        public override async Task OnConnectedAsync()
        {
            var chats = await _service.ChatService.GetAccountChatsAsync(Context.User.Claims.Where(x => x.Type == "userId").First().Value);
            foreach (var chat in chats)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chat.ConnectionId.ToString());
            }
            
            await base.OnConnectedAsync();
        }

    }
}
