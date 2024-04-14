using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class ChatMemberNotFoundException : NotFoundException
    {
        public ChatMemberNotFoundException(Guid accountId, Guid chatId) : base($"the account {accountId} is not a member of the chat {chatId}")
        {

        }

    }
}
