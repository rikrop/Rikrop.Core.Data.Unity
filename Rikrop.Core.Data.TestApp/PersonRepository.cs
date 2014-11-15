using System;
using Rikrop.Core.Data.Repositories.Contracts;
using Rikrop.Core.Data.Unity.Attributes;
using Rikrop.Core.Data.Unity.Repositories;

namespace Rikrop.Core.Data.TestApp
{
    [Repository(typeof(IPersonRepository))]
    public class PersonRepository : DeactivatableRepository<Person, int>, IPersonRepository
    {
        public PersonRepository(IRepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        #region Implementation of IPersonRepository

        public void ExtensionMethod()
        {
            Console.WriteLine("PersonRepository ExtensionMethod called");
        }

        #endregion
    }
}
