using Entities.ExtendedModels;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account> GetAccountByIdAsync(Guid accountId);
        Task<AccountExtended> GetAccountWithDetailsAsync(Guid accountId);
        Task CreateAccountAsync(Account account);
        Task UpdateAccountAsync(Account dbAccount, Account account);
        Task DeleteAccountAsync(Account account);
        object AccountsByOwner(Guid id);
    }
}
