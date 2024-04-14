using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.ChannelDto
{
    public record ChannelMemberDto
    {
        public Guid Id { get; init; }
        public string? Bio { get; init; }
        public string UserName { get; init; }
        public string? Image { get; init; }
        public string? ChannelRole { get; set; }
    }
}
