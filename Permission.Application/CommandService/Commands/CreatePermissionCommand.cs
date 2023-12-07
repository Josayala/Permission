using Permission.Application.Abstractions;
using Permission.Infrastructure.Dtos;

namespace Permission.Application.CommandService.Commands
{ 
    public class CreatePermissionCommand : ICommand<PermissionDto>
    {    
        public string EmployeeForename { get; set; }
        public string EmployeeSurname { get; set; }
        public int PermissionTypeId { get; set; }

    }
}
