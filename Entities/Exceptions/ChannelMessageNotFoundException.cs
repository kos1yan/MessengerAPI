using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class ChannelMessageNotFoundException : NotFoundException
    {
        public ChannelMessageNotFoundException(Guid channelMessageId) : base($"The message with id: {channelMessageId} doesn't exist in the database.")
        {

        }

    }
}
