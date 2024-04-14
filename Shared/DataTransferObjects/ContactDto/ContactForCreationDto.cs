using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.ContactDto
{
    public record ContactForCreationDto
    {
        [Required(ErrorMessage = "FriendContactId is a required field.")]
        public Guid? FriendContactId { get; init; }
    }
}
