using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects.AccountDto;
using Shared.DataTransferObjects.ChannelDto;
using Shared.DataTransferObjects.ChannelMessageDto;
using Shared.DataTransferObjects.ChatDto;
using Shared.DataTransferObjects.ChatMessageDto;
using Shared.DataTransferObjects.ContactDto;
using Shared.DataTransferObjects.UserDto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Messenger
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountDto>();
            CreateMap<Chat, ChatDto>();
            CreateMap<Channel, ChannelDto>();
            CreateMap<Account, ChatMemberDto>();
            CreateMap<Account, ChannelMemberDto>();
            CreateMap<ChatMessage, ChatMessageDto>();
            CreateMap<ChannelMessage, ChannelMessageDto>();
            CreateMap<ContactForCreationDto, Contact>();
            CreateMap<ChatMessageForCreationDto, ChatMessage>();
            CreateMap<ChannelMessageForCreationDto, ChannelMessage>();
            CreateMap<ChatForCreationDto, Chat>().ForMember(s => s.Image, opt => opt.Ignore());
            CreateMap<ChannelForCreationDto, Channel>().ForMember(s => s.Image, opt => opt.Ignore());
            CreateMap<AccountForUpdateDto, Account>().ForMember(s => s.Image, opt => opt.Ignore());
            CreateMap<ChatForUpdateDto, Chat>().ForMember(s => s.Image, opt => opt.Ignore());
            CreateMap<ChannelForUpdateDto, Channel>().ForMember(s => s.Image, opt => opt.Ignore());
            CreateMap<UserForRegistrationDto, User>().ForMember(s => s.UserName, opt => opt.MapFrom(x => x.Email));
            CreateMap<ChatMessageForUpdateDto, ChatMessage>().ForMember(s => s.Image, opt => opt.Ignore());
        }
    }
}
