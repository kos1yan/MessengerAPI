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
using System.Threading.Tasks;

namespace Repository
{
    public class ChatRepository : RepositoryBase<Chat>, IChatRepository
    {

        public ChatRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }
        
        public async Task<PagedList<Chat>> GetChatsAsync(ChatParameters chatParameters, bool trackChanges)
        {
            var chats = await FindAll(trackChanges).Search(chatParameters.SearchTerm)
                .OrderBy(c => c.Name)
                .Skip((chatParameters.PageNumber - 1) * chatParameters.PageSize)
                .Take(chatParameters.PageSize)
                .ToListAsync();
            var count = await FindAll(trackChanges).Search(chatParameters.SearchTerm).CountAsync();


            return new PagedList<Chat>(chats, count, chatParameters.PageNumber, chatParameters.PageSize);

        }
        public async Task<PagedList<Chat>> GetAccountChatsAsync(Guid accountId, ChatParameters chatParameters)
        {
            var account = await _repositoryContext.Accounts.Include(c => c.Chats).Where(c =>c.Id.Equals(accountId)).SingleOrDefaultAsync();
            var chats = account.Chats.Search(chatParameters.SearchTerm)
                .OrderBy(c => c.Name)
                .Skip((chatParameters.PageNumber - 1) * chatParameters.PageSize)
                .Take(chatParameters.PageSize)
                .ToList();
            
            return new PagedList<Chat>(chats, chats.Count, chatParameters.PageNumber, chatParameters.PageSize);
        }
        public async Task<IEnumerable<Chat>> GetAccountChatsAsync(string accountId)
        {
            var account = await _repositoryContext.Accounts.Include(c => c.Chats).Where(c => c.Id.ToString().Equals(accountId)).SingleOrDefaultAsync();
            return account.Chats;
        }
        public async Task<Chat> GetChatAsync(Guid chatId, bool trackChanges)
        {
            return await FindByCondition(c => c.Id.Equals(chatId), trackChanges).SingleOrDefaultAsync();
        }

        public void CreateChat(Chat chat, Account account)
        {
            Create(chat);
            chat.ChatMembers.Add(new ChatMember { AccountId = account.Id, ChatRole = ChatRoles.Admin });
        }
        public void DeleteChat(Chat chat) => Delete(chat);

        public async Task<ChatMember> GetChatMemberAsync(Guid accountId, Guid chatId)
        {
            return await _repositoryContext.ChatMembers.FindAsync(accountId, chatId);
        }

        public void DeleteChatMember(ChatMember chatMember)
        {
            _repositoryContext.ChatMembers.Remove(chatMember);
        }

        public async Task<List<Account>> GetChatMembersAccountsAsync(Guid chatId, bool trackChanges)
        {
            var chat = await FindByCondition(c => c.Id.Equals(chatId), trackChanges).Include(c =>c.Accounts).SingleOrDefaultAsync();

            return chat.Accounts;

        }

    }
}
