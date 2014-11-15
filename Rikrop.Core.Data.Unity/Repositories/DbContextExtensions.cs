using System.Data.Entity;

namespace Rikrop.Core.Data.Unity.Repositories
{
    /// <summary>
    /// Методы-расширения для класса DbContext.
    /// </summary>
    internal static class DbContextExtensions
    {
        /// <summary>
        /// Извлечение данных из БД.
        /// </summary>
        /// <typeparam name="TEntity">Тип извлекаемых данных.</typeparam>
        /// <param name="dbContext">Контекст БД.</param>
        internal static DbSet<TEntity> GetData<TEntity>(this DbContext dbContext) 
            where TEntity : class
        {
            return dbContext.Set<TEntity>();
        }
    }
}
