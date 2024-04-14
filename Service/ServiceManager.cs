using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.Contracts;
using Shared.DataTransferObjects.AccountDto;
using Shared.DataTransferObjects.ChannelDto;
using Shared.DataTransferObjects.ChatDto;
using Shared.DataTransferObjects.ContactDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAccountService> _accountService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IChatService> _chatService;
        private readonly Lazy<IChannelService> _channelService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper, IDataShaper<AccountDto> dataShaper,
        UserManager<User> userManager,
        IOptions<JwtConfiguration> configuration,
        IDataShaper<ChatDto> dataShaperChat,
        IDataShaper<ChannelDto> dataShaperChannel)
        {
            _accountService = new Lazy<IAccountService>(() => new AccountService(repositoryManager,loggerManager, mapper, dataShaper));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(loggerManager, mapper, userManager, configuration, repositoryManager));
            _chatService = new Lazy<IChatService>(() => new ChatService(repositoryManager, loggerManager, mapper, dataShaperChat));
            _channelService = new Lazy<IChannelService>(() => new ChannelService(repositoryManager, loggerManager, mapper, dataShaperChannel));


        }

        public IAccountService AccountService => _accountService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public IChatService ChatService => _chatService.Value;
        public IChannelService ChannelService => _channelService.Value;


    }
}
