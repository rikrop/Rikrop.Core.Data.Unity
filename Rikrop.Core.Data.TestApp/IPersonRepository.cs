using Rikrop.Core.Data.Repositories.Contracts;

namespace Rikrop.Core.Data.TestApp
{
    public interface IPersonRepository : IDeactivatableRepository<Person, int>
    {
        void ExtensionMethod();
    }
}
