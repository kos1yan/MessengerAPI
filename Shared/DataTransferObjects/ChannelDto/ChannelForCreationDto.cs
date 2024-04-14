using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.ChannelDto
{
    public record ChannelForCreationDto
    {
        [Required(ErrorMessage = "ConnectionId is a required field.")]
        public Guid? ConnectionId { get; set; }
        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(50, ErrorMessage = "Maximum name length 50 characters!")]
        public string? Name { get; init; }
        public string? Description { get; init; }
        public IFormFile? Image { get; init; }
    }
}
