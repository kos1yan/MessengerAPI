using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IChannelMessageRepository
    {
        Task<PagedList<ChannelMessage>> GetChannelMessagesAsync(Guid channelId, ChannelMessageParameters channelMessageParameters, bool trackChanges);
        Task<ChannelMessage> GetChannelMessageAsync(Guid channelMessageId, bool trackChanges);
        void CreateChannelMessage(Guid channelId, ChannelMessage message);
        void DeleteChannelMessage(ChannelMessage message);
    }
}
