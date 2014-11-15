using System;
using Rikrop.Core.Data.Repositories.Contracts;

namespace Rikrop.Core.Data.Unity.Repositories
{
    /// <summary>
    /// Генератор кода репозитория по типу интерфейса.
    /// </summary>
    internal class RepositoryGenerator
    {
        #region Открытые методы

        /// <summary>
        /// Генерация класса репозритория.
        /// </summary>
        /// <param name="repositoryInterfaceType">Тип интерфейса репозитория.</param>
        /// <returns>Тип сгенерированного репозитория.</returns>
        public Type Generate(Type repositoryInterfaceType)
        {
            if (!repositoryInterfaceType.IsInterface)
            {
                throw new RepositoryGeneratorException(
                    string.Format("Ожидается интерфейс, но получен класс {0}.", repositoryInterfaceType.Name));
            }

            var repositoryType = DefineType(repositoryInterfaceType);

            return repositoryType;
        }

        

        #endregion Открытые методы

        #region Скрытые методы

        private static Type DefineType(Type repositoryInterfaceType)
        {
            Type repositoryType = null;
            Type[] genericTypeArgs = null;
            if (repositoryInterfaceType.InheritsFromGeneric(typeof(IDeactivatableRepository<,>)))
            {
                repositoryType = typeof(DeactivatableRepository<,>);
                genericTypeArgs = repositoryInterfaceType.GetGenericTypeArguments(typeof (IDeactivatableRepository<,>));
            }
            else if (repositoryInterfaceType.InheritsFromGeneric(typeof(IRepository<,>)))
            {
                repositoryType = typeof(Repository<,>);
                genericTypeArgs = repositoryInterfaceType.GetGenericTypeArguments(typeof(IRepository<,>));
            }

            if (repositoryType == null)
                throw new NotSupportedException();

            Type constructed = repositoryType.MakeGenericType(genericTypeArgs);

            return constructed;
        }

        #endregion Скрытые методы
    }
}
