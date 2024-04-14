using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Service.Contracts;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Presentation.Hubs
{
    [Authorize]
    public class ChannelHub : Hub<IChannelClient>
    {
        private readonly IServiceManager _service;
        public ChannelHub(IServiceManager service)
        {
            _service = service;
        }
        public async Task SendMessageToChannel(ChannelHubMessageParameters message)
        {
            await Clients.Group(message.ConnectionId.ToString()).ReceiveMessage(message.Text, message.Image);
        }

        public async Task AddToChannel(ChannelHubGroupsParameters parameters)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, parameters.ConnectionId.ToString());

            await Clients.Group(parameters.ConnectionId.ToString()).ReceiveMessage($"{parameters.AccountName} has joined the channel.");

        }

        public async Task RemoveFromChannel(ChannelHubGroupsParameters parameters)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, parameters.ConnectionId.ToString());

            await Clients.Group(parameters.ConnectionId.ToString()).ReceiveMessage($"{parameters.AccountName} has left the channel.");
        }

        public override async Task OnConnectedAsync()
        {
            var channels = await _service.ChannelService.GetAccountChannelsAsync(Context.UserIdentifier);
            foreach (var channel in channels)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, channel.ConnectionId.ToString());
            }

            await base.OnConnectedAsync();
        }

    }
}
