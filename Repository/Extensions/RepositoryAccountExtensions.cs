using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class RepositoryAccountExtensions
    {
        public static IQueryable<Account> Search(this IQueryable<Account> accounts, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return accounts;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return accounts.Where(e => e.UserName.ToLower().Contains(lowerCaseTerm));
        }

    }
}
