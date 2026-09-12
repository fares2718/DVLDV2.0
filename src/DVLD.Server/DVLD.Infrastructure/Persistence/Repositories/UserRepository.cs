using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.DTOs;
using DVLD.Application.Features.User.AccountStatus;
using DVLD.Application.Filters.User;
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

    public async Task AddRoleAsync(UserRole role,CancellationToken cancellationToken = default)
    {
        var result = await _dvldContext.Roles
            .Where(r => r.RoleId == role.RoleId && r.IsActive)
            .Select(r => new
            {
                RoleExists = true,

                TargetUserExists = _dvldContext.Users
                    .Any(u => u.UserId == role.UserId),

                AssigningUserExists = _dvldContext.Users
                    .Any(u => u.UserId == role.AssignedBy),
                
                UserHasRole = _dvldContext.UserRoles
                    .Where(ur => ur.UserId == role.UserId)
                    .Any(ur => ur.RoleId == role.RoleId),
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (result is null)
            throw new KeyNotFoundException(
                $"Active role with ID {role.RoleId} was not found");

        if (!result.TargetUserExists)
            throw new KeyNotFoundException(
                $"Target user with ID {role.UserId} was not found");

        if (!result.AssigningUserExists)
            throw new KeyNotFoundException(
                $"Assigning user with ID {role.AssignedBy} was not found");
        
        if (result.UserHasRole)
            throw new DomainException($"User with ID {role.UserId} already has role with ID {role.RoleId}");


        _dvldContext.UserRoles.Add(role);
    }

    public async Task RemoveRoleAsync(Guid userId, int roleId,CancellationToken cancellationToken = default)
    {
        var userRole = await _dvldContext.UserRoles.Where(ur => ur.UserId == userId)
            .SingleOrDefaultAsync(ur => ur.RoleId == roleId, cancellationToken);

        if (userRole is null)
            throw new DomainException($"User with ID : {userId} does not have role with ID {roleId}");
        
        _dvldContext.Entry(userRole).State = EntityState.Deleted;
    }

    public async Task<UserDetailsView?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dvldContext.UserDetailsViews
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.UserId == userId, cancellationToken);
    }

    public async Task<UserDetailsView?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dvldContext.UserDetailsViews
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<PagedList<UserView>> GetUsersAsync(GetUsersFilter filter, CancellationToken cancellationToken = default)
    {
        IQueryable<UserView> query = _dvldContext.UsersViews.AsNoTracking();

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
        int totalCount = 0;
        
        if (items.Any())
            totalCount = items.Count;
        
        return new PagedList<UserView>(
            items,
            totalCount,
            filter.PageNumber,
            filter.PageSize);
    }

    private async Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken = default)
    {
        return !await _dvldContext.UserDetailsViews
            .AsNoTracking()
            .AnyAsync(u => u.Username == username, cancellationToken);
    }

    private async Task<bool> ExistsByPersonIdAsync(Guid personId, CancellationToken cancellationToken = default)
    {
        return await _dvldContext.UserDetailsViews
            .AsNoTracking()
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

    public async Task ChangeUserLockStatusAsync(Guid userId, bool isLocked, LockDurationType? durationType,
        int? duration,CancellationToken cancellationToken = default)
    {
        var user = await GetAsync(userId, cancellationToken);
        if(user is null)
            throw new KeyNotFoundException($"User with User ID : {userId} was not found");
        if (isLocked)
        {
            DateTime lockedUntil;
            switch (durationType)
            {
                case LockDurationType.Minutes or null:
                    lockedUntil = DateTime.UtcNow.AddMinutes(duration ?? 3);
                    break;
                case LockDurationType.Hours:
                    lockedUntil = DateTime.UtcNow.AddHours(duration ?? 1);
                    break;
                case LockDurationType.Days:
                    lockedUntil = DateTime.UtcNow.AddDays(duration ?? 1);
                    break;
                case LockDurationType.Months:
                    lockedUntil = DateTime.UtcNow.AddMonths(duration ?? 1);
                    break;
                case LockDurationType.Years:
                    lockedUntil = DateTime.UtcNow.AddYears(duration ?? 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(durationType), durationType, null);
            }
            user.Lock(lockedUntil);   
        }
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
        return await _dvldContext.UserRoles
            .AsNoTracking()
            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId,
            cancellationToken);
    }
}