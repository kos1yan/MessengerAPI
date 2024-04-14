using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.ChatMessageDto
{
    public record ChatMessageForCreationDto
    {
        public string? Text { get; init; }
        public IFormFile? Image { get; init; }
        [Required(ErrorMessage = "Account id is required!")]
        public Guid? AccountId { get; init; }
    }
}
