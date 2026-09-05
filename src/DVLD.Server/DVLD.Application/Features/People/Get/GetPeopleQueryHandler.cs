using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.DTOs;
using DVLD.Domain.Views;
using MediatR;
using ErrorOr;

namespace DVLD.Application.Features.People.Get;

public class GetPeopleQueryHandler : IRequestHandler<GetPeopleQuery,ErrorOr<PagedList<PersonSummary>>>
{
    private readonly IUnitOfWork _uow;

    public GetPeopleQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<ErrorOr<PagedList<PersonSummary>>> Handle(GetPeopleQuery request, CancellationToken cancellationToken)
    {
        return await _uow.PersonRepository.GetPeopleSummaryAsync(request,cancellationToken);
    }
}