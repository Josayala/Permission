using Permission.Application.Abstractions;
using Permission.Infrastructure.Dtos;

namespace Permission.Application.QueryService.Queries
{
    public sealed record GetPermissionsQuery : IQuery<List<PermissionDto>>;
}
