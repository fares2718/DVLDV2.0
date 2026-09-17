using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Persistence.Repositories;

internal sealed class RoleRepository(DvldContext dvldContext) : IRoleRepository
{
    private readonly DvldContext _dvldContext = dvldContext;

    public async Task<long> GetRolePermissions(List<string> userRoles)
    {
        List<Role> roles = await _dvldContext.Set<Role>().Where(r => userRoles.Contains(r.Name))
            .AsNoTracking()
            .ToListAsync();

        long permissions = roles
            .Select(x => x.Permissions)
            .Aggregate(0L, (current, rolePermissions) =>
                current | rolePermissions);
        return permissions;
    }
}