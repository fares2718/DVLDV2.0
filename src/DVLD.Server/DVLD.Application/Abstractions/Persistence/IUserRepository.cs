using DVLD.Application.DTOs;
using DVLD.Application.Filters;
using DVLD.Domain.Entities;
using DVLD.Domain.Views;

namespace DVLD.Application.Abstractions.Persistence;

public interface IUserRepository
{
    // Create
    Task AddAsync(User user,CancellationToken cancellationToken = default);

    // Roles
    void AddRole(UserRole role);
    
    void AddRoles(IEnumerable<UserRole> userRoles);
    Task RemoveRoleAsync(Guid userId, int roleId);

    // Queries
    Task<UserDetailsView?> GetByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<UserDetailsView?> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);
    
    Task<PagedList<UserView>> GetUsersAsync(
    GetUsersFilter filter,
    CancellationToken cancellationToken = default);

    // Account status
    Task ChangeUserActivationStatusAsync(
        Guid userId,
        bool isActive,
        CancellationToken cancellationToken = default);

    Task ChangeUserLockStatusAsync(
        Guid userId,
        bool isLocked,
        CancellationToken cancellationToken = default);
    
}