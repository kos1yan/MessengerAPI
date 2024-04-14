using Contracts;
using Entities.ConfigurationModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IAccountRepository> _accountRepository;
        private readonly Lazy<IContactRepository> _contactRepository;
        private readonly Lazy<IPhotoRepository> _photoRepository;
        private readonly Lazy<IChatRepository> _chatRepository;
        private readonly Lazy<IChatMessageRepository> _chatMessageRepository;
        private readonly Lazy<IChannelRepository> _channelRepository;
        private readonly Lazy<IChannelMessageRepository> _channelMessageRepository;

        public RepositoryManager(RepositoryContext repositoryContext, IOptions<CloudinarySettings> cloudinarySettings)
        {
            _repositoryContext = repositoryContext;
            _accountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(repositoryContext));
            _contactRepository = new Lazy<IContactRepository>(() => new ContactRepository(repositoryContext));
            _photoRepository = new Lazy<IPhotoRepository>(() => new PhotoRepository(cloudinarySettings));
            _chatRepository = new Lazy<IChatRepository>(() => new ChatRepository(repositoryContext));
            _chatMessageRepository = new Lazy<IChatMessageRepository>(() => new ChatMessageRepository(repositoryContext));
            _channelRepository = new Lazy<IChannelRepository>(() => new ChannelRepository(repositoryContext));
            _channelMessageRepository = new Lazy<IChannelMessageRepository>(() => new ChannelMessageRepository(repositoryContext));


        }
        public IAccountRepository Account => _accountRepository.Value;
        public IContactRepository Contact => _contactRepository.Value;
        public IPhotoRepository Photo => _photoRepository.Value;
        public IChatRepository Chat => _chatRepository.Value;
        public IChatMessageRepository ChatMessage => _chatMessageRepository.Value;
        public IChannelRepository Channel => _channelRepository.Value;
        public IChannelMessageRepository ChannelMessage => _channelMessageRepository.Value;

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();

    }
}
