using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.ChatDto
{
    public record ChatDto
    {
        public Guid Id { get; init; }
        public Guid ConnectionId { get; init; }
        public string Name { get; init; }
        public string? Image { get; init; }
        public IEnumerable<ChatMemberDto> MembersAccounts { get; set; }
        
    }
}
