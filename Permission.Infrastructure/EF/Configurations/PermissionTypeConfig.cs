using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Permission.Domain.Models;

namespace Permission.Infrastructure.EF.Configurations
{
    public class PermissionTypeConfig : IEntityTypeConfiguration<PermissionType>
    {
        public void Configure(EntityTypeBuilder<PermissionType> entity)
        {
            entity.HasKey(e => e.Id).IsClustered(false);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.ToTable("PermissionTypes");
            entity.Property(e => e.Description).HasMaxLength(50);
        }
    }
}
