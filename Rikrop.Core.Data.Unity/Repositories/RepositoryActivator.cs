using System;
using Microsoft.Practices.Unity;
using Rikrop.Core.Data.Repositories.Contracts;

namespace Rikrop.Core.Data.Unity.Repositories
{
    /// <summary>
    /// Активатор экземпляра репозитория по типу.
    /// </summary>
    internal class RepositoryActivator
    {
        /// <summary>
        /// Получение экземпляра репозитория из типа объекта.
        /// </summary>
        /// <param name="container">Unity-container.</param>
        /// <param name="concreteRepositoryType">Тип объекта репозитория.</param>
        /// <returns>Экземпляр класса репозритория.</returns>
        public object CreateInstance(IUnityContainer container, Type concreteRepositoryType)
        {
            var constructor = concreteRepositoryType.GetConstructor(new[] { typeof(IRepositoryContext) });
            if (constructor == null)
            {
                throw new RepositoryGeneratorException(
                    "Generated repository doesn't have public constructor with IRepositoryContext parameter. " +
                    "There's something wrong with RepositoryGenerator.");
            }

            return constructor.Invoke(new object[] { container.Resolve<IRepositoryContext>() });
        }
    }
}
