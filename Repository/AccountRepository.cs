using Contracts;
using Contracts;
using Entities;
using Entities.ExtendedModels;
using Entities.Extensions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public async Task CreateAccountAsync(Account account)
        {
            account.Id = Guid.NewGuid();
            Create(account);
            await SaveAsync();
        }

        public async Task DeleteOwnerAsync(Account account)
        {
            Delete(account);
            await SaveAsync();
        }

        public async Task<IEnumerable<Owner>> GetAllAccountsAsync()
        {
            return (IEnumerable<Owner>)await FindAll().OrderBy(a => a.Name).ToListAsync();  
        }

        public async Task<Account> GetAccountByIdAsync(Guid accountId)
        {
            try
            {
                var account = await FindByCondition(o => o.Id.Equals(accountId)).SingleAsync();
                return account;
            }
            catch
            {
                return new Account();
            }
        }

        public async Task<OwnerExtended> GetAccountWithDetailsAsync(Guid accountId)
        {
            try
            {
                var account = await FindByCondition(o => o.Id.Equals(accountId)).SingleAsync();
                var accountExtended = new Extended(account)
                {
                    Accounts = RepositoryContext.Accounts.Where(a => a.AccountId == accountId)
                };
                return accountExtended;
            }
            catch
            {
                return new AccountExtended();
            }
        }

        public async Task UpdateAccountAsync(Account dbAccount, Account account)
        {
            dbAccount.Map(account);
            Update(dbAccount);
            await SaveAsync();
        }

        Task<IEnumerable<Account>> IAccountRepository.GetAllAccountsAsync()
        {
            throw new NotImplementedException();
        }

        Task<AccountExtended> IAccountRepository.GetAccountWithDetailsAsync(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAccountAsync(Account account)
        {
            throw new NotImplementedException();
        }
    }
}
