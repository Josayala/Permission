using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Permission.Domain.Models;

namespace Permission.Infrastructure.EF.Configurations
{
    public class PermissionConfig : IEntityTypeConfiguration<Domain.Models.Permission>
    {
        public void Configure(EntityTypeBuilder<Domain.Models.Permission> entity)
        {
          
            entity.HasKey(e => e.Id).IsClustered(false);
            entity.ToTable("Permissions");
            entity.Property(e => e.EmployeeForename).HasMaxLength(50);
            entity.Property(e => e.EmployeeSurname).HasMaxLength(50);
            entity.Property(e => e.PermissionTypeId);
            entity.Property(e => e.PermissionDate).HasColumnType("datetime2(7)");
            entity.HasOne(e => e.PermissionTypes).WithMany(e => e.Permissions)
            .HasForeignKey(e => e.PermissionTypeId)
            .OnDelete(DeleteBehavior.Restrict);
            
        }
    }

}
