using Microsoft.EntityFrameworkCore;
using Permission.Domain.Interfaces;

namespace Permission.Infrastructure.EF.Repositories
{
    public class PermissionRepository : Repository, IPermissionRepository
    { 
        public PermissionRepository(PermissionDbContext context) : base(context)
        {      
        }

        public IQueryable<Domain.Models.Permission> GetBaseQuery()
        {
            return GetBaseQuery(default);
        }

        public IQueryable<Domain.Models.Permission> GetBaseQuery(CancellationToken cancellationToken)
        {
            return Find<Domain.Models.Permission>(cancellationToken)
                .Include(x => x.PermissionTypes);
        }

      
    }
}
