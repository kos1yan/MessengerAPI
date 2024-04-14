using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures
{
    public class ChannelHubMessageParameters
    {
        public string? Text { get; init; }
        public string? Image { get; init; }
        public Guid? ConnectionId { get; init; }
    }
}
