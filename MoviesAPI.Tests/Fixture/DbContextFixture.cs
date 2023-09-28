using MoviesAPI.Infrastructure.Data;
using MoviesAPI.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Tests.Fixture
{
    public class DbContextFixture : IDisposable
    {
        public AppDbContext DbContext { get; private set; }

        public DbContextFixture()
        {
            DbContext = TestDbContextFactory.Create();
            TestDataSeeder.Seed(DbContext);
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
