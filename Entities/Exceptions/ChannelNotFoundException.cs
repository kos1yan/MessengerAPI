using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class ChannelNotFoundException : NotFoundException
    {
        public ChannelNotFoundException(Guid channelId) : base($"The channel with id: {channelId} doesn't exist in the database.")
        {

        }

    }
}
