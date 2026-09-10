using DVLD.Application.Abstractions.Persistence;

namespace DVLD.Infrastructure.Persistence.Repositories;

internal class UnitOfWork(IPersonRepository personRepository, IAddressRepository addressRepository) : IUnitOfWork
{
    public IPersonRepository PersonRepository { get; } = personRepository;
    public IAddressRepository AddressRepository { get;  } = addressRepository;
}