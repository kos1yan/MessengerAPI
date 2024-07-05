using Entities.Models;
using Shared.DataTransferObjects.AccountDto;
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
    public interface IChatService
    {
        Task<(IEnumerable<ExpandoObject> chats, MetaData metaData)> GetChatsAsync(ChatParameters chatParameters, bool trackChanges);
        Task<(IEnumerable<ExpandoObject> chats, MetaData metaData)> GetAccountChatsAsync(Guid accountId, ChatParameters chatsParameters, bool trackChanges);
        Task<IEnumerable<ChatDto>> GetAccountChatsAsync(string accountId);
        Task<ChatDto> GetChatAsync(Guid chatId, bool trackChanges);
        Task<ChatDto> CreateChatAsync(Guid accountId, ChatForCreationDto chatForCreation, bool trackChanges);
        Task DeleteChatAsync(Guid chatId, bool trackChanges);
        Task LeaveChatAsync(Guid accountId, Guid chatId, bool trackChanges);
        Task UpdateChatAsync(Guid chatId, ChatForUpdateDto chatForUpdate, bool chatTrackChanges, bool accountTrackChanges);

        Task<(Dictionary<string, List<ChatMessageDto>> chatMessages, MetaData metaData)> GetChatMessagesAsync(Guid chatId, ChatMessageParameters chatMessageParameters, bool trackChanges);
        Task<ChatMessageDto> GetChatMessageAsync(Guid messageId, bool trackChanges);
        Task<ChatMessageDto> CreateChatMessageAsync(Guid chatId, ChatMessageForCreationDto messageForCreation, bool trackChanges);
        Task DeleteChatMessageAsync(Guid chatMessageId, bool trackChanges);
        Task AddMemberToChatAsync(Guid accountId, Guid chatId, bool chatTrackChanges, bool accountTrackChanges);
        Task UpdateChatMessageAsync(Guid chatMessageId, ChatMessageForUpdateDto chatMessageForUpdate, bool trackChanges);
    }
}
