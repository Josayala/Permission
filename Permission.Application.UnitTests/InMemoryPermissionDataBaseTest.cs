using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Permission.Infrastructure.EF.Repositories;

namespace Permission.Application.UnitTests
{
    public abstract class InMemoryPermissionDataBaseTest : IDisposable
    {
        public PermissionDbContext DbContext { get; private set; }
        protected InMemoryPermissionDataBaseTest(string databaseName)
        {
            var dbContextOptions = new DbContextOptionsBuilder<PermissionDbContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            this.DbContext = new PermissionDbContext(dbContextOptions);
        }
        public abstract void SeedDatabase();

        public void Dispose()
        {
            this.DbContext.Database.EnsureDeleted();
            this.DbContext.Dispose();
        }
    }
}
