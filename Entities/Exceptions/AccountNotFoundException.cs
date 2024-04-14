using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class AccountNotFoundException : NotFoundException
    {
        public AccountNotFoundException(Guid accountId) : base($"The account with id: {accountId} doesn't exist in the database.") 
        {

        }

    }
}
