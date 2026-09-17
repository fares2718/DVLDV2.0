namespace DVLD.Application.Abstractions.Persistence;

public interface IRoleRepository
{
    Task<long> GetRolePermissions(List<string> userRoles);
}