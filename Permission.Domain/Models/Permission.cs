
namespace Permission.Domain.Models
{
    public class Permission
    {
        private int id;
        private string employeeForename;
        private string employeeSurname;
        private int permissionTypeId;
        private DateTime permissionDate;
        private PermissionType permissionTypes;


        protected Permission()
        {
        }

        public Permission(int id, string employeeForename, string employeeSurname, int permissionTypeId , DateTime permissionDate) : this()
        {   
            this.id = id;
            this.employeeForename = employeeForename.Trim();
            this.employeeSurname = employeeSurname.Trim();
            this.permissionTypeId = permissionTypeId;
            this.permissionDate = permissionDate;

        }

        public PermissionType PermissionTypes { get => permissionTypes; private set => permissionTypes = value; }
        public int Id { get => id; private set => id = value; }
        public string EmployeeForename { get => employeeForename; private set => employeeForename = value; }
        public string EmployeeSurname { get => employeeSurname; private set => employeeSurname = value; }
        public int PermissionTypeId { get => permissionTypeId; private set => permissionTypeId = value; }
        public DateTime PermissionDate { get => permissionDate; private set => permissionDate = value; }

        public void Update(string employeeForename, string employeeSurname, int permissionTypeId)
        {
            this.employeeForename = employeeForename.Trim();
            this.employeeSurname = employeeSurname.Trim();
            this.permissionTypeId = permissionTypeId;
            this.permissionDate = DateTime.Now;
        }

        public void UpdatePermissionType(PermissionType permissionType)
        {
            this.permissionTypes = permissionType;
        }
    }
}
