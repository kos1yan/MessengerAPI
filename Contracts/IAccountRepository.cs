using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Contracts
{
    public interface IAccountRepository
    {
        Task<PagedList<Account>> GetAccountsAsync(AccountParameters accountParameters, bool trackChanges);
        Task<Account> GetAccountAsync ( Guid accountId, bool trackChanges);
        void CreateAccount (Account account);
        void DeleteAccount (Account account);
       
    }
}
