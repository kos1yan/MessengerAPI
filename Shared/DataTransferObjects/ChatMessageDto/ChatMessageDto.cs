using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.ChatMessageDto
{
    public record ChatMessageDto
    {
        public Guid Id { get; init; }
        public string? Text { get; init; }
        public string? Image { get; init; }
        public DateTime PublicationTime { get; init; }
        public Guid AccountId { get; init; }
    }
}
