using Microsoft.EntityFrameworkCore;
using Permission.Domain.Interfaces;
using Permission.Infrastructure.EF.Repositories;

namespace Permission.Infrastructure
{   
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PermissionDbContext _context;
        private PermissionRepository _permissionRepository;

        public UnitOfWork(PermissionDbContext context)
        {
            _context = context;
        }

        public IPermissionRepository PermissionRepository
        {
            get
            {
                if (_permissionRepository == null)
                {
                    _permissionRepository = new PermissionRepository(_context);
                }
                return _permissionRepository;
            }
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
