using AutoMapper;
using CloudinaryDotNet;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
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

namespace Service
{
    internal sealed class ChatService : IChatService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<ChatDto> _dataShaper;

        public ChatService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<ChatDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }


        public async Task<(IEnumerable<ExpandoObject> chats, MetaData metaData)> GetChatsAsync(ChatParameters chatParameters, bool trackChanges)
        {
            var chatsWithMetaData = await _repository.Chat.GetChatsAsync(chatParameters, trackChanges);
            var chatsDto = _mapper.Map<IEnumerable<ChatDto>>(chatsWithMetaData);
            var shapedData = _dataShaper.ShapeData(chatsDto, chatParameters.Fields);


            return (chats: shapedData, metaData: chatsWithMetaData.MetaData);
        }
        public async Task<(IEnumerable<ExpandoObject> chats, MetaData metaData)> GetAccountChatsAsync(Guid accountId, ChatParameters chatsParameters, bool trackChanges)
        {
            var account = await _repository.Account.GetAccountAsync(accountId, trackChanges);
            if (account is null) throw new AccountNotFoundException(accountId);

            var chatsWithMetaData = await _repository.Chat.GetAccountChatsAsync(accountId, chatsParameters);
            var chatsDto = _mapper.Map<IEnumerable<ChatDto>>(chatsWithMetaData);

            foreach (var chatDto in chatsDto)
            {
                chatDto.MembersAccounts = _mapper.Map<IEnumerable<ChatMemberDto>>(await _repository.Chat.GetChatMembersAccountsAsync(chatDto.Id, trackChanges));

                foreach (var member in chatDto.MembersAccounts)
                {
                    var chatMember = await _repository.Chat.GetChatMemberAsync(member.Id, chatDto.Id);
                    member.ChatRole = chatMember.ChatRole;
                }
            }

            var shapedData = _dataShaper.ShapeData(chatsDto, chatsParameters.Fields);

            return (chats: shapedData, metaData: chatsWithMetaData.MetaData);
        }

        public async Task<IEnumerable<ChatDto>> GetAccountChatsAsync(string accountId)
        {
            var chats = await _repository.Chat.GetAccountChatsAsync(accountId);
            return _mapper.Map<IEnumerable<ChatDto>>(chats);
        }

        public async Task<ChatDto> GetChatAsync(Guid chatId, bool trackChanges)
        {
            var chat = await _repository.Chat.GetChatAsync(chatId, trackChanges);

            if (chat is null) throw new ChatNotFoundException(chatId);

            var chatDto = _mapper.Map<ChatDto>(chat);

            chatDto.MembersAccounts = _mapper.Map<IEnumerable<ChatMemberDto>>(await _repository.Chat.GetChatMembersAccountsAsync(chatId, trackChanges));

            foreach (var member in chatDto.MembersAccounts)
            {
                var chatMember = await _repository.Chat.GetChatMemberAsync(member.Id, chat.Id);
                member.ChatRole = chatMember.ChatRole;
            }
            return chatDto;
        }

        public async Task<ChatDto> CreateChatAsync(Guid accountId, ChatForCreationDto chatForCreation, bool trackChanges)
        {
            var account = await _repository.Account.GetAccountAsync(accountId, trackChanges);
            if (account is null) throw new AccountNotFoundException(accountId);
            var chat = _mapper.Map<Chat>(chatForCreation);
            chat.ConnectionId = Guid.NewGuid();
            if (chatForCreation.Image != null)
            {
                var result = await _repository.Photo.AddPhotoAsync(chatForCreation.Image);
                chat.Image = result.Url.ToString();
            }
            _repository.Chat.CreateChat(chat, account);
            await _repository.SaveAsync();
            var chatToReturn = _mapper.Map<ChatDto>(chat);
            return chatToReturn;
        }

        public async Task DeleteChatAsync(Guid chatId, bool trackChanges)
        {
            var chat = await _repository.Chat.GetChatAsync(chatId, trackChanges);
            if (chat is null) throw new ChatNotFoundException(chatId);
            if (chat.Image != null) await _repository.Photo.DeletePhotoAsync(chat.Image);
            _repository.Chat.DeleteChat(chat);
            await _repository.SaveAsync();
        }

        public async Task UpdateChatAsync(Guid chatId, ChatForUpdateDto chatForUpdate, bool chatTrackChanges, bool accountTrackChanges)
        {
            var chat = await _repository.Chat.GetChatAsync(chatId, chatTrackChanges);
            if (chat is null) throw new ChatNotFoundException(chatId);
            if (chatForUpdate.Image != null)
            {
                if (chat.Image != null) await _repository.Photo.DeletePhotoAsync(chat.Image);
                var result = await _repository.Photo.AddPhotoAsync(chatForUpdate.Image);
                chat.Image = result.Url.ToString();
            }
            _mapper.Map(chatForUpdate, chat);
            await _repository.SaveAsync();
        }

        public async Task LeaveChatAsync(Guid accountId, Guid chatId, bool trackChanges)
        {
            var account = await _repository.Account.GetAccountAsync(accountId, trackChanges);
            if (account is null) throw new AccountNotFoundException(accountId);
            var chat = await _repository.Chat.GetChatAsync(chatId, trackChanges);
            if (chat is null) throw new ChatNotFoundException(chatId);
            var chatMember = await _repository.Chat.GetChatMemberAsync(accountId, chatId);
            if (chatMember is null) throw new ChatMemberNotFoundException(accountId, chatId);
            _repository.Chat.DeleteChatMember(chatMember);
            if (chatMember.ChatRole == ChatRoles.Admin) _repository.Chat.DeleteChat(chat);
            await _repository.SaveAsync();
        }

        public async Task<(IEnumerable<ChatMessageDto> chatMessages, MetaData metaData)> GetChatMessagesAsync(Guid chatId, ChatMessageParameters chatMessageParameters, bool trackChanges)
        {
            var chat = await _repository.Chat.GetChatAsync(chatId, trackChanges);
            if (chat is null) throw new ChatNotFoundException(chatId);
            var chatMessagesWithMetaData = await _repository.ChatMessage.GetChatMessagesAsync(chatId, chatMessageParameters, trackChanges);
            var chatMessagesDto = _mapper.Map<IEnumerable<ChatMessageDto>>(chatMessagesWithMetaData);

            foreach (var message in chatMessagesDto)
            {
                var account = await _repository.Account.GetAccountAsync(message.AccountId, false);
                if (account is null) continue;
                
                message.AccountName = account.UserName;
                message.AccountImage = account.Image;
            }

            return (chatMessages: chatMessagesDto, metaData: chatMessagesWithMetaData.MetaData);
        }
        public async Task<ChatMessageDto> CreateChatMessageAsync(Guid chatId, ChatMessageForCreationDto messageForCreation, bool trackChanges)
        {
            var chat = await _repository.Chat.GetChatAsync(chatId, trackChanges);
            if (chat is null) throw new ChatNotFoundException(chatId);
            var message = _mapper.Map<ChatMessage>(messageForCreation);
            if (messageForCreation.Image != null)
            {
                var result = await _repository.Photo.AddPhotoAsync(messageForCreation.Image);
                message.Image = result.Url.ToString();
            }
            _repository.ChatMessage.CreateChatMessage(chatId, message);
            await _repository.SaveAsync();

            var chatMessagetoReturn = _mapper.Map<ChatMessageDto>(message);
            
            return chatMessagetoReturn;
        }
        public async Task DeleteChatMessageAsync(Guid chatMessageId, bool trackChanges)
        {
            var chatMessage = await _repository.ChatMessage.GetChatMessageAsync(chatMessageId, trackChanges);
            if (chatMessage is null) throw new ChatMessageNotFoundException(chatMessageId);
            if (chatMessage.Image != null) await _repository.Photo.DeletePhotoAsync(chatMessage.Image);
            _repository.ChatMessage.DeleteChatMessage(chatMessage);
            await _repository.SaveAsync();
        }

        public async Task<ChatMessageDto> GetChatMessageAsync(Guid messageId, bool trackChanges)
        {
            var message = await _repository.ChatMessage.GetChatMessageAsync(messageId, trackChanges);
            if(message is null) throw new ChatMessageNotFoundException(messageId);
            var messageDto = _mapper.Map<ChatMessageDto>(message);
            return messageDto;
        }

        public async Task AddMemberToChatAsync(Guid accountId, Guid chatId, bool chatTrackChanges, bool accountTrackChanges)
        {
            var account = await _repository.Account.GetAccountAsync(accountId, accountTrackChanges);
            if (account is null) throw new AccountNotFoundException(accountId);
            var chat = await _repository.Chat.GetChatAsync(chatId, chatTrackChanges);
            if (chat is null) throw new ChatNotFoundException(chatId);
            chat.ChatMembers.Add(new ChatMember { ChatId = chatId, AccountId = accountId, ChatRole = ChatRoles.Member });
            await _repository.SaveAsync();
        }

        public async Task UpdateChatMessageAsync(Guid chatMessageId, ChatMessageForUpdateDto chatMessageForUpdate, bool trackChanges)
        {
            var chatMessage = await _repository.ChatMessage.GetChatMessageAsync(chatMessageId, trackChanges);
            if(chatMessage is null) throw new ChatMessageNotFoundException(chatMessageId);

            if (chatMessageForUpdate.Image != null)
            {
                if(chatMessage.Image != null) await _repository.Photo.DeletePhotoAsync(chatMessage.Image);
                var result = await _repository.Photo.AddPhotoAsync(chatMessageForUpdate.Image);
                chatMessage.Image = result.Url.ToString();
            }

            _mapper.Map(chatMessageForUpdate, chatMessage);
            await _repository.SaveAsync();
        }
    }
}
