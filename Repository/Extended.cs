using Entities.Models;
using System.Linq;

namespace Repository
{
    internal class Extended
    {
        private Account account;

        public Extended(Account account)
        {
            this.account = account;
        }

        public IQueryable<Account> Accounts { get; set; }
    }
}