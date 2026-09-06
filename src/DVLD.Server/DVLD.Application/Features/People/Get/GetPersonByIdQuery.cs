using DVLD.Domain.Views;
using MediatR;
using ErrorOr;

namespace DVLD.Application.Features.People.Get;

public sealed record GetPersonByIdQuery(Guid PersonId) : IRequest<ErrorOr<PersonSummary>>;