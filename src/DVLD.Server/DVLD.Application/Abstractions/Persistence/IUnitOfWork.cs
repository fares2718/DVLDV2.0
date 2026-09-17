using DVLD.Application.Abstractions.Authentication;

namespace DVLD.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    //Repositories
    public IPersonRepository PersonRepository { get; }
    public IAddressRepository AddressRepository { get;  }
    public IUserRepository UserRepository { get; }
    public IUserRefreshTokenRepository UserRefreshTokenRepository { get; }
    public IRoleRepository RoleRepository { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}