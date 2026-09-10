using DVLD.Application.Abstractions.Persistence;

namespace DVLD.Infrastructure.Persistence.Repositories;

internal class UnitOfWork(IPersonRepository personRepository, IAddressRepository addressRepository, IUnitOfWork uow) : IUnitOfWork
{
    private readonly IUnitOfWork _uow = uow;
    public IPersonRepository PersonRepository { get; } = personRepository;
    public IAddressRepository AddressRepository { get;  } = addressRepository;
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await  _uow.SaveChangesAsync(cancellationToken);
    }
}