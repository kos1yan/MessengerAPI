using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects.AccountDto;
using Shared.DataTransferObjects.ContactDto;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Extensions;
using System.Dynamic;

namespace Service
{
    internal sealed class AccountService : IAccountService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<AccountDto> _dataShaper;

        public AccountService ( IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<AccountDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }


        public async Task<(IEnumerable<ExpandoObject> accounts, MetaData metaData)> GetAccountsAsync(AccountParameters accountParameters, bool trackChanges)
        {
            var accountsWithMetaData = await _repository.Account.GetAccountsAsync(accountParameters, trackChanges);
            var accountsDto = _mapper.Map<IEnumerable<AccountDto>>(accountsWithMetaData);
            var shapedData = _dataShaper.ShapeData(accountsDto, accountParameters.Fields);


            return (accounts: shapedData, metaData: accountsWithMetaData.MetaData);
        }
        public async Task<AccountDto> GetAccountAsync(Guid accountId, bool trackChanges)
        {
            var account = await _repository.Account.GetAccountAsync(accountId, trackChanges);
            if (account is null) throw new AccountNotFoundException(accountId);
            var accountDto = _mapper.Map<AccountDto>(account);
            return accountDto;
        }

        public async Task<(IEnumerable<ExpandoObject> contacts, MetaData metaData)> GetAccountContactsAsync(Guid accountId, ContactParameters contactParameters, bool trackChanges)
        {
            var account = await _repository.Account.GetAccountAsync(accountId, trackChanges);
            if (account is null) throw new AccountNotFoundException(accountId);
            var contactsWithMetaData = await _repository.Contact.GetAccountContactsAsync(accountId, contactParameters, trackChanges);
            var contactList = new List<AccountDto>();
            foreach ( var contact in contactsWithMetaData)
            {
                var contactAccount = await _repository.Account.GetAccountAsync(contact.FriendContactId, trackChanges);
                contactList.Add(_mapper.Map<AccountDto>(contactAccount));
            }
            var contactsSearch = contactList.Search(contactParameters.SearchTerm);
            var shapedData = _dataShaper.ShapeData(contactsSearch, contactParameters.Fields);
            return (contacts : shapedData, metaData : contactsWithMetaData.MetaData);
        }

        public async Task CreateAccountContactAsync(Guid accountId, ContactForCreationDto contact, bool trackChanges)
        {
            var account = await _repository.Account.GetAccountAsync(accountId, trackChanges);
            if (account is null) throw new AccountNotFoundException(accountId);
            if (account.Id == contact.FriendContactId) throw new CreateAccountContactBadRequestException();
            var contactEntity = _mapper.Map<Contact>(contact);
            _repository.Contact.CreateAccountContact(accountId, contactEntity);
            await _repository.SaveAsync();
        }

        public async Task DeleteAccountContactAsync(Guid accountId, Guid id, bool trackChanges)
        {
            var account = await _repository.Account.GetAccountAsync(accountId, trackChanges);
            if (account is null) throw new AccountNotFoundException(accountId);
            var contact = await _repository.Contact.GetAccountContactAsync(accountId, id, trackChanges);
            if(contact is null) throw new ContactNotFoundException(id);
            _repository.Contact.DeleteAccountContact(contact);
            await _repository.SaveAsync();
        }

        public async Task DeleteAccountAsync(Guid accountId, bool trackChanges)
        {
            var account = await _repository.Account.GetAccountAsync(accountId, trackChanges);
            if (account is null) throw new AccountNotFoundException(accountId);
            if (account.Image != null) await _repository.Photo.DeletePhotoAsync(account.Image);
            _repository.Account.DeleteAccount(account);
            var friendsContacts = await _repository.Contact.GetContactsByFriendIdAsync(accountId, trackChanges);
            foreach (var contact in friendsContacts)
            {
                _repository.Contact.DeleteAccountContact(contact);
            }
            await _repository.SaveAsync();
        }

        public async Task UpdateAccountAsync(Guid accountId, AccountForUpdateDto account, bool trackChanges)
        {
            var accountEntity = await _repository.Account.GetAccountAsync(accountId, trackChanges);
            if(accountEntity is null) throw new AccountNotFoundException(accountId);
            if(account.Image != null)
            {
                var result = await _repository.Photo.AddPhotoAsync(account.Image);
                accountEntity.Image = result.Url.ToString();
            }
            _mapper.Map(account, accountEntity);
            await _repository.SaveAsync();
        }


    }
}
