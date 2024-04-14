using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        IAccountRepository Account { get;}
        IContactRepository Contact { get;}
        IPhotoRepository Photo { get;}
        IChatRepository Chat{ get;}
        IChatMessageRepository ChatMessage{ get;}
        IChannelRepository Channel { get; }
        IChannelMessageRepository ChannelMessage { get; }
        Task SaveAsync();

    }
}
