namespace DVLD.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    public IPersonRepository PersonRepository { get; }
}