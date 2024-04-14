using Shared.DataTransferObjects.AccountDto;
using Shared.DataTransferObjects.ContactDto;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IAccountService
    {
        Task<(IEnumerable<ExpandoObject> accounts, MetaData metaData)> GetAccountsAsync(AccountParameters accountParameters, bool trackChanges);
        Task<AccountDto> GetAccountAsync(Guid accountId, bool trackChanges);
        Task<(IEnumerable<ExpandoObject> contacts, MetaData metaData)> GetAccountContactsAsync(Guid accountId, ContactParameters contactParameters, bool trackChanges);
        Task CreateAccountContactAsync(Guid accountId, ContactForCreationDto contact, bool trackChanges);
        Task DeleteAccountContactAsync(Guid accountId, Guid id, bool trackChanges);
        Task DeleteAccountAsync(Guid accountId, bool trackChanges);
        Task UpdateAccountAsync(Guid accountId, AccountForUpdateDto account, bool trackChanges);
    }
}
