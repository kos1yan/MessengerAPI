using CloudinaryDotNet;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }
        
        public async Task<PagedList<Contact>> GetAccountContactsAsync(Guid accountId, ContactParameters contactParameters, bool trackChanges)
        {
            var contacts = await FindByCondition(c => c.AccountId.Equals(accountId), trackChanges)
                .OrderBy(c => c.AddTime)
                .Skip((contactParameters.PageNumber - 1) * contactParameters.PageSize)
                .Take(contactParameters.PageSize)
                .ToListAsync();
            var count = await FindByCondition(c => c.AccountId.Equals(accountId), trackChanges).CountAsync();

            return new PagedList<Contact>(contacts, count, contactParameters.PageNumber, contactParameters.PageSize);
        }

        public async Task<IEnumerable<Contact>> GetContactsByFriendIdAsync(Guid friendContactId, bool trackChanges)
        {
            return await FindByCondition(c => c.FriendContactId.Equals(friendContactId), trackChanges).ToListAsync();
        }

        public async Task<Contact> GetAccountContactAsync(Guid accountId, Guid id, bool trackChanges)
        {
            return await FindByCondition(c => c.AccountId.Equals(accountId) && c.FriendContactId.Equals(id), trackChanges).SingleOrDefaultAsync();
        }

        public void CreateAccountContact(Guid accountId, Contact contact)
        {
            contact.AccountId = accountId;
            Create(contact);
        }

        public void DeleteAccountContact(Contact contact) => Delete(contact);

    }
}
