using Shared.DataTransferObjects.AccountDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class RepositoryContactExtensions
    {
        public static IEnumerable<AccountDto> Search(this List<AccountDto> contactList, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return contactList;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return contactList.Where(e => e.UserName.ToLower().Contains(lowerCaseTerm));
        }
    }
}
