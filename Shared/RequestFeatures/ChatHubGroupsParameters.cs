using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures
{
    public class ChatHubGroupsParameters
    {
        public Guid? AccountId {  get; init; }
        public Guid? AccountIdToAdd {  get; init; }
        public Guid? ChatId {  get; init; }
        public Guid? ConnectionId {  get; init; }
    }
}
