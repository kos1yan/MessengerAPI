using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ChatMessageRepository : RepositoryBase<ChatMessage>, IChatMessageRepository 
    {
        public ChatMessageRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<PagedList<ChatMessage>> GetChatMessagesAsync(Guid chatId, ChatMessageParameters chatMessageParameters, bool trackChanges)
        {
            var chatMessages = await FindByCondition(c => c.ChatId.Equals(chatId),trackChanges)
                            .OrderByDescending(c =>c.PublicationTime)
                            .Skip((chatMessageParameters.PageNumber - 1) * chatMessageParameters.PageSize)
                            .Take(chatMessageParameters.PageSize)
                            .ToListAsync();

            var count = await FindByCondition(c => c.ChatId.Equals(chatId), trackChanges).CountAsync();

            return new PagedList<ChatMessage>(chatMessages, count, chatMessageParameters.PageNumber, chatMessageParameters.PageSize);
        }
        public async Task<ChatMessage> GetChatMessageAsync(Guid chatMessageId, bool trackChanges)
        {
            return await FindByCondition(c => c.Id.Equals(chatMessageId), trackChanges).SingleOrDefaultAsync();
        }
        public void CreateChatMessage(Guid chatId, ChatMessage message)
        {
            message.ChatId = chatId;
            Create(message);
        }
        public void DeleteChatMessage(ChatMessage message) => Delete(message);
    }
}
