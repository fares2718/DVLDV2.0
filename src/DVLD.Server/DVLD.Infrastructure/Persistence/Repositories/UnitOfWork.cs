using DVLD.Application.Abstractions.Persistence;
using DVLD.Infrastructure.Persistence.Context;

namespace DVLD.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public IPersonRepository PersonRepository { get; }

    public UnitOfWork(IPersonRepository personRepository)
    {
        PersonRepository = personRepository;
    }
}