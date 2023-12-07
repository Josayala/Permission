using Permission.Domain.Interfaces;

namespace Permission.Persistence.Repositories
{

    public class PermissionRepository : IPermissionRepository
    {
        private readonly PermissionDbContext _context;

        public PermissionRepository(PermissionDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Domain.Models.Permission permission)
        {
            throw new NotImplementedException();
        }

        public void Delete(Domain.Models.Permission permission)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Models.Permission>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Models.Permission> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Domain.Models.Permission permission)
        {
            throw new NotImplementedException();
        }
    }
}
