using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    

    public interface IChatClient
    {
        Task ReceiveMessage(Guid? user, string? message, string? image, Guid? chatId, Guid? messageId);
        Task DeleteNotification(Guid? chatId, Guid? messageId);
        Task EditNotification(Guid? accountId, Guid? chatId, Guid? messageId, string? text, string? image);
        Task AddToGroup(Guid? accountIdToAdd, Guid? accountId, Guid? chatId);
        Task RemoveFromGroup(Guid? accountId, Guid? chatId);
        Task DeleteGroup(Guid? chatId);

    }
}
