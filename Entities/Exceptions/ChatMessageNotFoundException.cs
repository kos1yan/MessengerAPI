using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class ChatMessageNotFoundException : NotFoundException
    {
        public ChatMessageNotFoundException(Guid chatMessageId) : base($"The message with id: {chatMessageId} doesn't exist in the database.")
        {

        }

    }
}
