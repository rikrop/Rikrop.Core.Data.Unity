using System.Data.Entity;
using Rikrop.Core.Data.Entities;
using Rikrop.Core.Data.Entities.Contracts;
using Rikrop.Core.Data.Repositories;
using Rikrop.Core.Data.Repositories.Contracts;

namespace Rikrop.Core.Data.Unity.Repositories
{
    public class DeactivatableRepository<TEntity, TId> : DeactivatableRepositoryBase<TEntity, TId>
        where TEntity : DeactivatableEntity<TId>, IRetrievableEntity<TEntity, TId>
        where TId : struct
    {
        public DeactivatableRepository(IRepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public override DbSet<TEntity> Data
        {
            get
            {
                return Context.GetData<TEntity>();
            }
        }
    }
}
