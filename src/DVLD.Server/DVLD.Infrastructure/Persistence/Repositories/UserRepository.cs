using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.DTOs;
using DVLD.Application.Filters;
using DVLD.Domain.Common;
using DVLD.Domain.Entities;
using DVLD.Domain.Views;
using DVLD.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Persistence.Repositories;

internal class UserRepository(DvldContext dvldContext) : IUserRepository
{
    private readonly DvldContext _dvldContext = dvldContext;

    public async Task AddAsync(User user,CancellationToken cancellationToken = default)
    {
        bool personExists = await ExistsByPersonIdAsync(user.PersonId,cancellationToken);
        if (personExists)
            throw new DomainException($"Person with Person ID {user.PersonId} is already a user");
        bool isUsernameUnique = await IsUsernameUniqueAsync(user.Username, cancellationToken);
        if (!isUsernameUnique)
            throw new DomainException($"Username {user.Username} is already a used");
        _dvldContext.Users.Add(user);
    }

    public void AddRole(UserRole role)
    {
        _dvldContext.UserRoles.Add(role);
    }

    public async Task RemoveRoleAsync(Guid userId, int roleId)
    {
        bool hasRole = await UserHasRole(userId, roleId);
        
        if(!hasRole)
            throw new KeyNotFoundException($"User with User ID : {userId} does not has role with ID : {roleId}");

        var userRole = UserHasRole(userId, roleId);
        
        _dvldContext.Entry(userRole).State = EntityState.Deleted;
    }

    public void AddRoles(IEnumerable<UserRole> userRoles)
    {
        _dvldContext.UserRoles.AddRange(userRoles);
    }

    public async Task<UserDetailsView?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dvldContext.UserDetailsViews
            .SingleOrDefaultAsync(u => u.UserId == userId, cancellationToken);
    }

    public async Task<UserDetailsView?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dvldContext.UserDetailsViews
            .SingleOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<PagedList<UserView>> GetUsersAsync(GetUsersFilter filter, CancellationToken cancellationToken = default)
    {
        IQueryable<UserView> query = _dvldContext.UsersViews;

        // Filtering
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();

            query = query.Where(x =>
                x.Username.Contains(search) ||
                x.FullName.Contains(search) ||
                x.NationalId.Contains(search) ||
                x.Phone.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(filter.Username))
        {
            var username = filter.Username.Trim();

            query = query.Where(x =>
                x.Username.Contains(username));
        }

        if (!string.IsNullOrWhiteSpace(filter.NationalId))
        {
            var nationalId = filter.NationalId.Trim();

            query = query.Where(x =>
                x.NationalId.Contains(nationalId));
        }

        if (!string.IsNullOrWhiteSpace(filter.Phone))
        {
            var phone = filter.Phone.Trim();

            query = query.Where(x =>
                x.Phone.Contains(phone));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(x =>
                x.IsActive == filter.IsActive.Value);
        }

        if (filter.IsLocked.HasValue)
        {
            query = query.Where(x =>
                x.IsLocked == filter.IsLocked.Value);
        }

        // Sorting
        query = filter.SortBy?.ToLower() switch
        {
            "username" => filter.IsDescending
                ? query.OrderByDescending(x => x.Username)
                : query.OrderBy(x => x.Username),

            "fullname" => filter.IsDescending
                ? query.OrderByDescending(x => x.FullName)
                : query.OrderBy(x => x.FullName),

            "nationalid" => filter.IsDescending
                ? query.OrderByDescending(x => x.NationalId)
                : query.OrderBy(x => x.NationalId),

            "phone" => filter.IsDescending
                ? query.OrderByDescending(x => x.Phone)
                : query.OrderBy(x => x.Phone),

            _ => query.OrderBy(x => x.Username)
        };

        // Pagination
        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<UserView>(
            items,
            items.Count,
            filter.PageNumber,
            filter.PageSize);
    }

    public async Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken = default)
    {
        return !await _dvldContext.UserDetailsViews
            .AnyAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<bool> ExistsByPersonIdAsync(Guid personId, CancellationToken cancellationToken = default)
    {
        return await _dvldContext.UserDetailsViews
            .AnyAsync(u => u.PersonId == personId, cancellationToken);
    }

    public async Task ChangeUserActivationStatusAsync(Guid userId, bool isActive, CancellationToken cancellationToken = default)
    {
        var user = await GetAsync(userId, cancellationToken);
        if(user is null)
            throw new KeyNotFoundException($"User with User ID : {userId} was not found");
        if(isActive)
            user.Activate();
        else
            user.Deactivate();
    }

    public async Task ChangeUserLockStatusAsync(Guid userId, bool isLocked, CancellationToken cancellationToken = default)
    {
        var user = await GetAsync(userId, cancellationToken);
        if(user is null)
            throw new KeyNotFoundException($"User with User ID : {userId} was not found");
        if(isLocked)
            user.Lock();
        else
            user.Unlock();
    }
    
    private async Task<User?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _dvldContext.Users.FindAsync(userId,cancellationToken);
        return user;
    }

    private async Task<bool> UserHasRole(Guid userId, int roleId, CancellationToken cancellationToken = default)
    {
        return await _dvldContext.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId,
            cancellationToken);
    }
}