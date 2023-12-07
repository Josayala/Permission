using Xunit;

namespace Permission.Application.UnitTests
{
    public class InMemoryPermissionDataBaseSeedData : InMemoryPermissionDataBaseTest
    {
        public InMemoryPermissionDataBaseSeedData() : base($"testdb_{Guid.NewGuid()}")
        {
            this.SeedDatabase();
        }

        public override void SeedDatabase()
        {
            var permission = new Domain.Models.Permission(1, "Juan", "Daniel", 1, DateTime.Now);
            var permissionType = new Domain.Models.PermissionType(1, "Read Only");
            this.DbContext.PermissionType.AddRange(permissionType);
            this.DbContext.Permissions.Add(permission);
            this.DbContext.SaveChanges();
        }
    }

    [CollectionDefinition("Context collection")]
    public class InMemoryDbContextFixtureCollection : ICollectionFixture<InMemoryPermissionDataBaseSeedData>
    {

    }
}
