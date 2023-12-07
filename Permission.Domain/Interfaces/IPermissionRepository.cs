namespace Permission.Domain.Interfaces
{
    public interface IPermissionRepository : IRepository
    {
        IQueryable<Domain.Models.Permission> GetBaseQuery();

        IQueryable<Domain.Models.Permission> GetBaseQuery(CancellationToken cancellationToken);

    }
}
