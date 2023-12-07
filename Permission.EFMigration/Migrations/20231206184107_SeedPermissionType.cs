using Microsoft.EntityFrameworkCore.Migrations;
using Permission.Domain.Models;

namespace Permission.EFMigration.Migrations
{
    public partial class SeedPermissionType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            List<PermissionType> permissionTypes = new List<PermissionType>
            {
                new PermissionType(1,"Read only"),
                new PermissionType(2,"Delete"),
                new PermissionType(3,"Modify"),
                new PermissionType(4,"Insert"),
            };

            foreach (var type in permissionTypes)
            {
                migrationBuilder.Sql($@"
                       If(Not Exists(Select 1 From PermissionTypes Where Id = '{type.Id}' and Description = '{type.Description}'))
                       Begin
                            Insert into PermissionTypes 
                            ([Id],[Description])
                            Values ('{type.Id}', '{type.Description}')
                       End
                       ");
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM PermissionTypes");
        }
    }
}
