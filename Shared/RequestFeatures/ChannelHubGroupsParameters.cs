using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures
{
    public class ChannelHubGroupsParameters
    {
        public string? AccountName { get; init; }
        public Guid? ConnectionId { get; init; }
    }
}
