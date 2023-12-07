using Permission.Application.Abstractions;
using Permission.Infrastructure.Dtos;

namespace Permission.Application.CommandService.Commands
{
    public class UpdatePermissionCommand : ICommand<PermissionDto>
    {
        public int Id { get; set; }
        public string EmployeeForename { get; set; }
        public string EmployeeSurname { get; set; }
        public int PermissionTypeId { get; set; }    
    }
}
