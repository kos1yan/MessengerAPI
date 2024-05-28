using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Channel = Entities.Models.Channel;

namespace Repository
{
    public class ChannelRepository : RepositoryBase<Channel>, IChannelRepository
    {

        public ChannelRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<PagedList<Channel>> GetChannelsAsync(ChannelParameters channelParameters, bool trackChanges)
        {
            var channels = await FindAll(trackChanges).Search(channelParameters.SearchTerm)
                .OrderBy(c => c.Name)
                .Skip((channelParameters.PageNumber - 1) * channelParameters.PageSize)
                .Take(channelParameters.PageSize)
                .ToListAsync();
            var count = await FindAll(trackChanges).Search(channelParameters.SearchTerm).CountAsync();


            return new PagedList<Channel>(channels, count, channelParameters.PageNumber, channelParameters.PageSize);
        }
        public async Task<PagedList<Channel>> GetAccountChannelsAsync(Guid accountId, ChannelParameters channelParameters)
        {
            var account = await _repositoryContext.Accounts.Include(c => c.Channels).Where(c => c.Id.Equals(accountId)).SingleOrDefaultAsync();
            var channels = account.Channels.Search(channelParameters.SearchTerm)
                .OrderBy(c => c.Name)
                .Skip((channelParameters.PageNumber - 1) * channelParameters.PageSize)
                .Take(channelParameters.PageSize)
                .ToList();
            var count = account.Channels.Search(channelParameters.SearchTerm).Count();

            return new PagedList<Channel>(channels, count, channelParameters.PageNumber, channelParameters.PageSize);
        }
        public async Task<IEnumerable<Channel>> GetAccountChannelsAsync(string accountId)
        {
            var account = await _repositoryContext.Accounts.Include(c => c.Channels).Where(c => c.Id.ToString().Equals(accountId)).SingleOrDefaultAsync();
            return account.Channels;
        }
        public async Task<Channel> GetChannelAsync(Guid channelId, bool trackChanges)
        {
            return await FindByCondition(c => c.Id.Equals(channelId), trackChanges).SingleOrDefaultAsync();
        }

        public void CreateChannel(Channel channel, Account account)
        {
            Create(channel);
            channel.ChannelMembers.Add(new ChannelMember { AccountId = account.Id, ChannelRole = ChannelRoles.Admin });
        }
        public void DeleteChannel(Channel channel) => Delete(channel);

        public async Task<ChannelMember> GetChannelMemberAsync(Guid accountId, Guid channelId)
        {
            return await _repositoryContext.ChannelMembers.FindAsync(accountId, channelId);
        }

        public void DeleteChannelMember(ChannelMember channelMember)
        {
            _repositoryContext.ChannelMembers.Remove(channelMember);
        }

        public async Task<List<Account>> GetChannelMembersAccountsAsync(Guid channelId, bool trackChanges)
        {
            var channel = await FindByCondition(c => c.Id.Equals(channelId), trackChanges).Include(c => c.Accounts).SingleOrDefaultAsync();

            return channel.Accounts;

        }

    }
}
