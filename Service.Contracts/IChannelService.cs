using Shared.DataTransferObjects.ChannelDto;
using Shared.DataTransferObjects.ChannelMessageDto;
using Shared.DataTransferObjects.ChatDto;
using Shared.DataTransferObjects.ChatMessageDto;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IChannelService
    {
        Task<(IEnumerable<ExpandoObject> channels, MetaData metaData)> GetChannelsAsync(ChannelParameters channelParameters, bool trackChanges);
        Task<(IEnumerable<ExpandoObject> channels, MetaData metaData)> GetAccountChannelsAsync(Guid accountId, ChannelParameters channelsParameters, bool trackChanges);
        Task<IEnumerable<ChannelDto>> GetAccountChannelsAsync(string accountId);
        Task<ChannelDto> GetChannelAsync(Guid channelId, bool trackChanges);
        Task<ChannelDto> CreateChannelAsync(Guid accountId, ChannelForCreationDto channelForCreation, bool trackChanges);
        Task DeleteChannelAsync(Guid channelId, bool trackChanges);
        Task LeaveChannelAsync(Guid accountId, Guid channelId, bool trackChanges);
        Task UpdateChannelAsync(Guid channelId, ChannelForUpdateDto channelForUpdate, bool channelTrackChanges, bool accountTrackChanges);

        Task<(IEnumerable<ChannelMessageDto> channelMessages, MetaData metaData)> GetChannelMessagesAsync(Guid channelId, ChannelMessageParameters channelMessageParameters, bool trackChanges);
        Task<ChannelMessageDto> GetChannelMessageAsync(Guid messageId, bool trackChanges);
        Task<ChannelMessageDto> CreateChannelMessageAsync(Guid channelId, ChannelMessageForCreationDto messageForCreation, bool trackChanges);
        Task DeleteChannelMessageAsync(Guid channelMessageId, bool trackChanges);
        Task AddMemberToChannelAsync(Guid accountId, Guid channelId, bool channelTrackChanges, bool accountTrackChanges);
    }
}
