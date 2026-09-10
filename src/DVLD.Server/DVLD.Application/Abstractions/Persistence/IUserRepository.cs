using DVLD.Domain.Entities;

namespace DVLD.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<Guid> CreateUserAsync(User user, CancellationToken cancellationToken = default);
}