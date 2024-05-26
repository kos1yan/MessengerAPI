using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures
{
    public class ChatHubMessageParameters
    {
        public Guid? Id { get; init; }   
        public string? Text {  get; init; }
        public string? Image {  get; init; }
        public Guid? AccountId {  get; init; }
        public Guid? ConnectionId {  get; init; }
        public Guid? ChatId { get; init; }
    }
}
