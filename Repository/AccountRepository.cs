using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<PagedList<Account>> GetAccountsAsync(AccountParameters accountParameters, bool trackChanges)
        {
            var accounts = await FindAll(trackChanges).Search(accountParameters.SearchTerm)
                .OrderBy(c =>c.UserName)
                .Skip((accountParameters.PageNumber - 1) * accountParameters.PageSize)
                .Take(accountParameters.PageSize)
                .ToListAsync();
            var count = await FindAll(trackChanges).Search(accountParameters.SearchTerm).CountAsync();


            return new PagedList<Account>(accounts, count, accountParameters.PageNumber, accountParameters.PageSize);
        }
        public async Task<Account> GetAccountAsync(Guid accountId, bool trackChanges)
        {
            return await FindByCondition(c => c.Id.Equals(accountId), trackChanges).SingleOrDefaultAsync();
        }

        public void CreateAccount(Account account) => Create(account);
        public void DeleteAccount(Account account) => Delete(account);
        
        
    }
}
