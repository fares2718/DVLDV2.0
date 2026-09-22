using DVLD.Application.Abstractions.Persistence;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Get;

public sealed class GetPeopleCountQueryHandler(IUnitOfWork uow) : IRequestHandler<GetPeopleCountQuery, ErrorOr<int>>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<int>> Handle(GetPeopleCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            int count = await _uow.PersonRepository.GetPeopleCount(cancellationToken);
            return count;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("Error",e.Message);
        }
    }
}