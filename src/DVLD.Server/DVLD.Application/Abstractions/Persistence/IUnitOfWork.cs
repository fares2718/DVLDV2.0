namespace DVLD.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    public IPersonRepository PersonRepository { get; }
    public IAddressRepository AddressRepository { get;  }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}