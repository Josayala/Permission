using Permission.Domain.Models;

namespace Permission.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPermissionRepository PermissionRepository { get; }
        void Commit();
    }
}
