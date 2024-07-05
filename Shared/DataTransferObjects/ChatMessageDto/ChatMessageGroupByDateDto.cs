using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.ChatMessageDto
{
    public record ChatMessageGroupByDateDto
    {
        public string? Date { get; set; }
        public List<ChatMessageDto> ChatMessages { get; set; }
    }
}
