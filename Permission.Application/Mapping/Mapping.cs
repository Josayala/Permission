using Nest;

namespace Permission.Application.Mapping
{
    public static class Mapping
    {
        public static CreateIndexDescriptor permissionMapping(this CreateIndexDescriptor descriptor) { 
         return descriptor.Map<Domain.Models.Permission>(m => m.Properties(p=>p.Keyword(k=>k.Name(n=>n.Id))
         .Text(t => t.Name(n => n.EmployeeSurname))
         .Text(t => t.Name(n => n.EmployeeForename))
         .Number(t => t.Name(n => n.PermissionTypeId))
         .Date(t => t.Name(n => n.PermissionDate))

         ));
        
        }
    }
}
