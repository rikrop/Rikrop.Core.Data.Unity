using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Rikrop.Core.Data.Repositories.Contracts;
using Rikrop.Core.Data.Unity;

namespace Rikrop.Core.Data.TestApp
{
    public class Program
    {
        static void Main()
        {
            var container = new UnityContainer();

            container.RegisterRepositoryContext<MyDbContext>();
            container.RegisterRepositoryContext(s => new MyDbContext(s), "myConStr");
            // Пример регистрации всех доступных в сборке репозиториев.
            container.RegisterRepositories(typeof(Department).Assembly);

            // Разрешение репозитория департаментов.
            var departmentRepository = container.Resolve<IRepository<Department, int>>();

            // Разрешшить IDeactivatableRepository для департамента нельзя (ошибка компиляции), 
            // т.к. эта сущность не относледована от DeactivatableEntity.
            //var departmentRepository2 = container.Resolve<IDeactivatableRepository<Department, int>>();

            // Для класса Person репозиторий зарегистрирован под обоими интерфейсами, поскольку сущность наследуется от DeactivatableEntity.
            var personRepository = container.Resolve<IRepository<Person, int>>();
            var personRepository2 = container.Resolve<IDeactivatableRepository<Person, int>>();

            var newDepartment = new Department {Name = Guid.NewGuid().ToString()};
            departmentRepository.Save(newDepartment);
            var allDeps = departmentRepository.GetAll();
            var testDeps = departmentRepository.Get(q => q.Filter(dep => dep.Name.Contains("Test")));

            allDeps.ForEach(Console.WriteLine);
            Console.WriteLine();

            departmentRepository.Delete(newDepartment);

            // Пример регистрации "расширенных" репозиториев без указания их типа.
            container.RegisterCustomRepositories(typeof(Department).Assembly);

            // Извлечение "расширенного" репозитория по интерфейсу.
            var personRepository3 = container.Resolve<IPersonRepository>();
            personRepository3.ExtensionMethod();

            Console.ReadKey();
        }
    }
}
