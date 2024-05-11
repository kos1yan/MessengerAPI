using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Repository
{
    public class ChannelMessageRepository : RepositoryBase<ChannelMessage>, IChannelMessageRepository
    {
        public ChannelMessageRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<PagedList<ChannelMessage>> GetChannelMessagesAsync(Guid channelId, ChannelMessageParameters channelMessageParameters, bool trackChanges)
        {
            var channelMessages = await FindByCondition(c => c.ChannelId.Equals(channelId), trackChanges)
                            .OrderByDescending(c => c.PublicationTime)
                            .Skip((channelMessageParameters.PageNumber - 1) * channelMessageParameters.PageSize)
                            .Take(channelMessageParameters.PageSize)
                            .ToListAsync();

            var count = await FindByCondition(c => c.ChannelId.Equals(channelId), trackChanges).CountAsync();

            return new PagedList<ChannelMessage>(channelMessages, count, channelMessageParameters.PageNumber, channelMessageParameters.PageSize);
        }
        public async Task<ChannelMessage> GetChannelMessageAsync(Guid channelMessageId, bool trackChanges)
        {
            return await FindByCondition(c => c.Id.Equals(channelMessageId), trackChanges).SingleOrDefaultAsync();
        }
        public void CreateChannelMessage(Guid channelId, ChannelMessage message)
        {
            message.ChannelId = channelId;
            Create(message);
        }
        public void DeleteChannelMessage(ChannelMessage message) => Delete(message);
    }
}
