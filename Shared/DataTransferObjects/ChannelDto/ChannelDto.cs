using Shared.DataTransferObjects.ChatDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.ChannelDto
{
    public record ChannelDto
    {
        public Guid Id { get; init; }
        public Guid ConnectionId { get; init; }
        public string? Description { get; init; }
        public string Name { get; init; }
        public string? Image { get; init; }
        public IEnumerable<ChannelMemberDto> MembersAccounts { get; set; }
    }
}
