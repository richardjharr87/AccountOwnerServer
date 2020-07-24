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
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public async Task CreateOwnerAsync(Owner owner)
        {
            owner.Id = Guid.NewGuid();
            Create(owner);
            await SaveAsync();
        }

        public async Task DeleteOwnerAsync(Owner owner)
        {
            Delete(owner);
            await SaveAsync();
        }

        public async Task<IEnumerable<Owner>> GetAllOwnersAsync()
        {
            return await FindAll().OrderBy(o => o.Name).ToListAsync();
        }

        public async Task<Owner> GetOwnerByIdAsync(Guid ownerId)
        {
            try
            {
                var owner = await FindByCondition(o => o.Id.Equals(ownerId)).SingleAsync();
                return owner;
            }
            catch
            {
                return new Owner();
            }
        }

        public async Task<OwnerExtended> GetOwnerWithDetailsAsync(Guid ownerId)
        {
            try
            {
                var owner = await FindByCondition(o => o.Id.Equals(ownerId)).SingleAsync();
                var ownerExtended = new OwnerExtended(owner)
                {
                    Accounts = RepositoryContext.Accounts.Where(a => a.OwnerId == ownerId)
                };
                return ownerExtended;
            }
            catch
            {
                return new OwnerExtended();
            }
        }

        public async Task UpdateOwnerAsync(Owner dbOwner, Owner owner)
            {
                dbOwner.Map(owner);
                Update(dbOwner);
                await SaveAsync();
            }
        }
    }
