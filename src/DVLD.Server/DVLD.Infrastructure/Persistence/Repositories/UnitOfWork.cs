using DVLD.Application.Abstractions.Persistence;
using DVLD.Infrastructure.Persistence.Context;

namespace DVLD.Infrastructure.Persistence.Repositories;

internal class UnitOfWork(IPersonRepository personRepository, IAddressRepository addressRepository, IUserRepository userRepository, DvldContext dvldContext) : IUnitOfWork
{
    private readonly DvldContext _dvldContext = dvldContext;
    public IPersonRepository PersonRepository { get; } = personRepository;
    public IAddressRepository AddressRepository { get;  } = addressRepository;
    public IUserRepository UserRepository { get; } = userRepository;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await  _dvldContext.SaveChangesAsync(cancellationToken);
    }
}