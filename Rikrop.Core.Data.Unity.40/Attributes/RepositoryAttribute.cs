using System;

namespace Rikrop.Core.Data.Unity.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RepositoryAttribute : Attribute
    {
        private readonly Type _repositoryInterfaceType;

        public Type RepositoryInterfaceType
        {
            get { return _repositoryInterfaceType; }
        }

        public RepositoryAttribute(Type repositoryInterfaceType)
        {
            if (repositoryInterfaceType == null)
                throw new ArgumentNullException("repositoryInterfaceType");

            _repositoryInterfaceType = repositoryInterfaceType;
        }
    }
}
