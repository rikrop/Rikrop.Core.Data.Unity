using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using Rikrop.Core.Data.Entities;
using Rikrop.Core.Data.Entities.Contracts;
using Rikrop.Core.Data.Repositories;
using Rikrop.Core.Data.Repositories.Contracts;
using Rikrop.Core.Data.Unity.Attributes;
using Rikrop.Core.Data.Unity.Repositories;

namespace Rikrop.Core.Data.Unity
{
    /// <summary>
    /// Методы-расширения для регистрации репозиториев доменных объектов в контейнере приложения.
    /// </summary>
    public static class RikropCoreDataUnityExtensions
    {
        #region Const

        private readonly static Type _repositoryInterfaceType = typeof(IRepository<,>);
        private readonly static Type _deactivatableRepositoryInterfaceType = typeof(IDeactivatableRepository<,>);
        private readonly static Type _deactivatableEntityType = typeof(DeactivatableEntity<>);
        private readonly static Type _retrievableEntityType = typeof(IRetrievableEntity<,>);

        #endregion Const

        #region Открытые методы

        /// <summary>
        /// Регистрация контекста БД для репозиториев.
        /// </summary>
        /// <typeparam name="TContext">Тип контекста БД.</typeparam>
        /// <param name="container">Unity-container.</param>
        public static void RegisterRepositoryContext<TContext>(this IUnityContainer container)
            where TContext : DbContext, new()
        {
            container.RegisterType<IRepositoryContext, RepositoryContext>(new InjectionFactory(c => new RepositoryContext(new TContext())));
        }

        /// <summary>
        /// Регистрация контекста БД для репозиториев.
        /// </summary>
        /// <typeparam name="TContext">Тип контекста БД.</typeparam>
        /// <param name="container">Unity-container.</param>
        /// <param name="contextConstructor">Конструктор контекста БД.</param>
        /// <param name="connectionString">Имя строки подключения.</param>
        public static void RegisterRepositoryContext<TContext>(this IUnityContainer container,
            Func<string, TContext> contextConstructor, string connectionString)
            where TContext : DbContext, new()
        {
            container.RegisterType<IRepositoryContext, RepositoryContext>(
                new InjectionFactory(c => new RepositoryContext(contextConstructor(connectionString))));
        }

        /// <summary>
        /// Автоматическая генерация и регистрация всех расширенных репозриториев.
        /// </summary>
        /// <param name="container">Unity-container.</param>
        /// <param name="assembly">Сборка, содержащая сущности репозиториев, помеченные атрибутом RepositoryAttribute.</param>
        public static void RegisterCustomRepositories(this IUnityContainer container, Assembly assembly)
        {
            foreach (var repositoryType in assembly.GetTypes().Where(type => type.IsClass))
            {
                var repositoryAttribute = repositoryType.GetCustomAttribute<RepositoryAttribute>();
                if (repositoryAttribute != null)
                {
                    container.RegisterType(
                        repositoryAttribute.RepositoryInterfaceType, 
                        repositoryType,
                        new TransientLifetimeManager());
                }
            }
        }

        /// <summary>
        /// Автоматическая генерация и регистрация всех репозриториев по Entity-сущностям.
        /// </summary>
        /// <param name="container">Unity-container.</param>
        /// <param name="assembly">Сборка, содержащая Entity-сущности, наследующие IRetrievableEntity.</param>
        public static void RegisterRepositories(this IUnityContainer container, Assembly assembly)
        {
            foreach (var entityType in assembly.GetTypes().Where(type => type.IsClass))
            {
                if (!entityType.InheritsFromGeneric(_retrievableEntityType))
                    continue;

                Type[] typeArgs = entityType.GetGenericTypeArguments(_retrievableEntityType);
                Type constructedRepositoryInterfaceType = _repositoryInterfaceType.MakeGenericType(typeArgs);
                container.RegisterRepository(constructedRepositoryInterfaceType);

                if (entityType.InheritsFrom(_deactivatableEntityType.MakeGenericType(new[] { typeArgs[1] })))
                {
                    var constructedDeactivatableRepositoryInterfaceType =
                        _deactivatableRepositoryInterfaceType.MakeGenericType(typeArgs);
                    container.RegisterRepository(constructedDeactivatableRepositoryInterfaceType);
                }
            }
        }

        #endregion Открытые методы

        #region Скрытые методы

        /// <summary>
        /// Генерация и регистрация репозриторя.
        /// </summary>
        /// <param name="container">Unity-container.</param>
        /// <param name="repositoryInterfaceType">Тип интерфейса репозитория.</param>
        private static void RegisterRepository(this IUnityContainer container, Type repositoryInterfaceType)
        {
            var factoryGenerator = new RepositoryGenerator();
            var concreteFactoryType = factoryGenerator.Generate(repositoryInterfaceType);
            container.RegisterType(
                repositoryInterfaceType,
                new TransientLifetimeManager(),
                new InjectionFactory(
                    c =>
                    {
                        var activator = new RepositoryActivator();
                        return activator.CreateInstance(c, concreteFactoryType);
                    }));
        }

        private static T GetCustomAttribute<T>(this Type type)
            where T : Attribute
        {
            var customAttributes = type.GetCustomAttributes(typeof(T), true);
            var customAttribute = customAttributes.SingleOrDefault() as T;
            return customAttribute;
        }

        #endregion Скрытые методы
    }
}
