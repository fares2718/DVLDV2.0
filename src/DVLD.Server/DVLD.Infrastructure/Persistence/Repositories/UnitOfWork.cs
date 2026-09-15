using DVLD.Application.Abstractions.Authentication;
using DVLD.Application.Abstractions.Persistence;
using DVLD.Infrastructure.Persistence.Context;

namespace DVLD.Infrastructure.Persistence.Repositories;

internal class UnitOfWork(IPersonRepository personRepository, IAddressRepository addressRepository, IUserRepository userRepository, DvldContext dvldContext, ITokensGenerator tokensGenerator, IUserRefreshTokenRepository userRefreshTokenRepository) : IUnitOfWork
{
    private readonly DvldContext _dvldContext = dvldContext;
    //JWT
    private readonly ITokensGenerator _tokensGenerator = tokensGenerator;
    
    //Repositories
    public IPersonRepository PersonRepository { get; } = personRepository;
    public IAddressRepository AddressRepository { get;  } = addressRepository;
    public IUserRepository UserRepository { get; } = userRepository;
    public IUserRefreshTokenRepository UserRefreshTokenRepository { get; } = userRefreshTokenRepository;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await  _dvldContext.SaveChangesAsync(cancellationToken);
    }
}