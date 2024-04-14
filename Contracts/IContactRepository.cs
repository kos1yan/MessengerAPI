using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IContactRepository
    {
        Task<PagedList<Contact>> GetAccountContactsAsync(Guid accountId, ContactParameters contactParameters, bool trackChanges);
        Task<IEnumerable<Contact>> GetContactsByFriendIdAsync(Guid friendContactId, bool trackChanges);
        Task<Contact> GetAccountContactAsync(Guid accountId, Guid id, bool trackChanges);
        void CreateAccountContact(Guid accountId, Contact contact);
        void DeleteAccountContact(Contact contact);
    }
}
