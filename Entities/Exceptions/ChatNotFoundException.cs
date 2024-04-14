using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class ChatNotFoundException : NotFoundException
    {
        public ChatNotFoundException(Guid chatId) : base($"The chat with id: {chatId} doesn't exist in the database.")
        {

        }

    }
}
