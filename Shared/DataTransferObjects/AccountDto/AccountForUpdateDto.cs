using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.AccountDto
{
    public record AccountForUpdateDto()
    {
        [MaxLength(200, ErrorMessage = "Maximum length for the Bio is 200 characters.")]
        public string? Bio { get; init; }
        [Required(ErrorMessage = "UserName is a required field.")]
        [MaxLength(50, ErrorMessage = " Maximum length for the UserName is 50 characters.")]
        public string? UserName { get; init; }
        public IFormFile? Image { get; init; }
    }
    
    
}
