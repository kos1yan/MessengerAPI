using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    

    public interface IChatClient
    {
        Task ReceiveMessage(Guid? user, string? message, string? Image, Guid? chatId);
        Task ReceiveMessage(string message);
    }
}
