using System.Data.Entity;

namespace Rikrop.Core.Data.TestApp
{
    public class MyDbContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }

        public MyDbContext()
            : base("MyConStr")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Database.Initialize(false);
        }
    }
}
