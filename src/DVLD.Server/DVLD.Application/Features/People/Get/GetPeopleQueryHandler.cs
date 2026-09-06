using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.DTOs;
using DVLD.Domain.Views;
using MediatR;
using ErrorOr;

namespace DVLD.Application.Features.People.Get;

public class GetPeopleQueryHandler : IRequestHandler<GetPeopleQuery,ErrorOr<PagedList<PersonSummary>>>
{
    private readonly IUnitOfWork _uow;
    private readonly GetPeopleQueryValidator _validator;

    public GetPeopleQueryHandler(IUnitOfWork uow, GetPeopleQueryValidator validator)
    {
        _uow = uow;
        _validator = validator;
    }

    public async Task<ErrorOr<PagedList<PersonSummary>>> Handle(GetPeopleQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if(!validationResult.IsValid)
            return Error.Validation("400",validationResult.Errors.First().ErrorMessage);
        var result = await _uow.PersonRepository.GetPeopleSummaryAsync(request,cancellationToken);
        if (result.Items.Count == 0)
            return Error.NotFound("404","No People was Found");
        return result;
    }
}