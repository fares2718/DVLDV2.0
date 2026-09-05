using DVLD.Application.Abstractions.Persistence;
using DVLD.Infrastructure.Persistence.Context;

namespace DVLD.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DvldContext _dvldContext;
    public IPersonRepository PersonRepository { get; }

    public UnitOfWork(DvldContext dvldContext, IPersonRepository personRepository)
    {
        _dvldContext = dvldContext;
        PersonRepository = personRepository;
    }
}