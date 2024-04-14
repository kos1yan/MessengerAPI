using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class CreateAccountContactBadRequestException : BadRequestException
    {
        public CreateAccountContactBadRequestException() : base("Account id and Contact id are the same") { }
        
    }
}
