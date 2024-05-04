using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.ChatMessageDto
{
    public record ChatMessageForUpdateDto
    {
        public string? Text { get; init; }
        public IFormFile? Image { get; init; }
    }
}
