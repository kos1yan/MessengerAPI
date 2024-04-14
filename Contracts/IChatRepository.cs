using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IChatRepository
    {
        Task<PagedList<Chat>> GetChatsAsync(ChatParameters chatParameters, bool trackChanges);
        Task<PagedList<Chat>> GetAccountChatsAsync(Guid accountId, ChatParameters chatParameters);
        Task<IEnumerable<Chat>> GetAccountChatsAsync(string accountId);
        Task<Chat> GetChatAsync(Guid chatId, bool trackChanges);
        void CreateChat(Chat chat, Account account);
        void DeleteChat(Chat chat);
        Task<ChatMember> GetChatMemberAsync(Guid accountId, Guid chatId);
        Task<List<Account>> GetChatMembersAccountsAsync(Guid chatId, bool trackChanges);
        void DeleteChatMember(ChatMember chatMember);

        
    }
}
