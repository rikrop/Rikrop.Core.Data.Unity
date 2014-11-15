using System;
using System.Collections.Generic;
using System.Linq;

namespace Rikrop.Core.Data.Unity
{
    /// <summary>
    /// Методы-расширения для класса Type.
    /// </summary>
    internal static class TypeExtensions
    {
        #region Поля

        private const char GenericTypeNameSeparator = '`';

        #endregion Поля

        #region Открытые методы

        public static bool InheritsFromGeneric(this Type type, Type baseGenericType)
        {
            var baseTypes = GetBaseTypes(type, baseGenericType, false);

            return baseTypes.Any();
        }

        public static bool InheritsFrom(this Type type, Type baseType)
        {
            var baseTypes = GetBaseTypes(type, baseType, true);

            return baseTypes.Any();
        }

        public static Type[] GetGenericTypeArguments(this Type type, Type baseType)
        {
            if(!baseType.IsGenericTypeDefinition)
                throw new ArgumentException(string.Format("Тип {0} не является определением Generic-типа", baseType.Name), baseType.Name);

            var baseTypes = GetBaseTypes(type, baseType, false);

            if (!baseTypes.Any())
                throw new ArgumentException(string.Format("Тип {0} не реализует базовый тип {1}", type.Name, baseType.Name));
            if(baseTypes.Count != 1)
                throw new NotSupportedException("Определение generic-параметров при реализации одного интерфейса с разными параметрами не реализовано");

            return baseTypes.Single().GenericTypeArguments;
        }

        #endregion Открытые методы

        #region Скрытые методы

        /// <summary>
        /// Получение списка базовых типов, реализованных в наследнике.
        /// </summary>
        /// <param name="type">Наследник.</param>
        /// <param name="baseType">Базовый тип.</param>
        /// <param name="concreteGenericTypes">Флаг конкретизации generic-параметров при сравнении типов.</param>
        /// <returns></returns>
        private static IReadOnlyCollection<Type> GetBaseTypes(Type type, Type baseType, bool concreteGenericTypes)
        {
            var baseTypes = GetBaseTypes(type);

            if (concreteGenericTypes)
            {
                return baseTypes.Where(bt => bt == baseType).ToArray();
            }

            var baseTypeName = baseType.Name.Trim(new[] { GenericTypeNameSeparator });
            return baseTypes.Where(t => t.Name.Trim(new[] { GenericTypeNameSeparator }) == baseTypeName).ToArray();
        }

        /// <summary>
        /// Получение списка иерархии наследования типа.
        /// </summary>
        /// <param name="type">Тип.</param>
        /// <returns>Полный список имплементируемых типов.</returns>
        private static IReadOnlyCollection<Type> GetBaseTypes(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var baseTypes = new List<Type> {type};

            baseTypes.AddRange(type.GetInterfaces());

            var currentType = type;
            while (currentType != null)
            {
                baseTypes.Add(currentType);
                currentType = currentType.BaseType;
            }

            return baseTypes.Distinct().ToArray();
        }

        #endregion Скрытые методы
    }
}
