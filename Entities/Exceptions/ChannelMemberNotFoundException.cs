using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class ChannelMemberNotFoundException : NotFoundException
    {
        public ChannelMemberNotFoundException(Guid accountId, Guid channelId) : base($"the account {accountId} is not a member of the channel {channelId}")
        {

        }

    }
}
