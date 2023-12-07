using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Permission.Infrastructure.EF.Repositories
{
    public class PermissionDbContext : DbContext
    {
        public PermissionDbContext(DbContextOptions<PermissionDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Domain.Models.Permission> Permissions => Set<Domain.Models.Permission>();
        public virtual DbSet<Domain.Models.PermissionType> PermissionType => Set<Domain.Models.PermissionType>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
