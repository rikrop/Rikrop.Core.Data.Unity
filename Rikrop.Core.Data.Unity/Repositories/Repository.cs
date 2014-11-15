using System.Data.Entity;
using Rikrop.Core.Data.Entities.Contracts;
using Rikrop.Core.Data.Repositories;
using Rikrop.Core.Data.Repositories.Contracts;

namespace Rikrop.Core.Data.Unity.Repositories
{
    public class Repository<TEntity, TId> : RepositoryBase<TEntity, TId>
        where TEntity : class, IRetrievableEntity<TEntity, TId>, IEntity<TId>
        where TId : struct
    {
        public Repository(IRepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        #region Реализация RepositoryBase<T,TId>

        public override DbSet<TEntity> Data
        {
            get
            {
                return Context.GetData<TEntity>();
            }
        }

        #endregion Реализация
    }
}
