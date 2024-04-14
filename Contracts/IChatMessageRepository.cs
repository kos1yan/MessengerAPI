using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IChatMessageRepository
    {
        Task<PagedList<ChatMessage>> GetChatMessagesAsync(Guid chatId, ChatMessageParameters chatMessageParameters, bool trackChanges);
        Task<ChatMessage> GetChatMessageAsync(Guid chatMessageId, bool trackChanges);
        void CreateChatMessage(Guid chatId, ChatMessage message);
        void DeleteChatMessage(ChatMessage message);
    }
}
