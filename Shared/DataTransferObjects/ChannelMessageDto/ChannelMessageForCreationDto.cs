using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.ChannelMessageDto
{
    public record ChannelMessageForCreationDto
    {
        public string? Text { get; init; }
        public IFormFile? Image { get; init; }
    }
}
