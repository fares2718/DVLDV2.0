using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Views;
using MediatR;
using ErrorOr;

namespace DVLD.Application.Features.People.Get;

public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery,ErrorOr<PersonSummary>>
{
    private readonly IUnitOfWork _uow;

    public GetPersonByIdQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<ErrorOr<PersonSummary>> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.PersonId == Guid.Empty)
            return Error.Validation("400","Person ID is required");

        var person = await _uow.PersonRepository.GetSummaryByIdAsync(request.PersonId,cancellationToken);
        if (person == null)
            return Error.NotFound("404",$"Person with ID : {request.PersonId} doesn't exist");
        return person;
    }
}