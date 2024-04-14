using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.AccountDto
{
    public record AccountDto
    {
        public Guid Id { get; init; }
        public string? Bio { get; init; }
        public string UserName { get; init; }
        public string? Image { get; init; }

    }
}
