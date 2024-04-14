using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IChannelRepository
    {
        Task<PagedList<Channel>> GetChannelsAsync(ChannelParameters channelParameters, bool trackChanges);
        Task<PagedList<Channel>> GetAccountChannelsAsync(Guid accountId, ChannelParameters channelParameters);
        Task<IEnumerable<Channel>> GetAccountChannelsAsync(string accountId);
        Task<Channel> GetChannelAsync(Guid channelId, bool trackChanges);
        void CreateChannel(Channel channel, Account account);
        void DeleteChannel(Channel channel);
        Task<ChannelMember> GetChannelMemberAsync(Guid accountId, Guid channelId);
        Task<List<Account>> GetChannelMembersAccountsAsync(Guid channelId, bool trackChanges);
        void DeleteChannelMember(ChannelMember channelMember);

    }
}
