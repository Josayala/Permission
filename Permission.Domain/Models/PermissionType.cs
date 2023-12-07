using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Domain.Models
{
    public class PermissionType
    {
        private int id;
        private string description;
        private ICollection<Permission> permissions;

        protected PermissionType()
        {
            permissions = new HashSet<Permission>();
        }

        public PermissionType(int id, string description) : this()
        {
            this.id = id;
            this.description = description.Trim();
        }


        public int Id { get => id; private set => id = value; }
        public string Description { get => description; private set => description = value; }

        public virtual ICollection<Permission> Permissions { get => permissions.ToList(); private set => permissions = value; }


    }
}
