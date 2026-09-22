using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Get;

public sealed record GetPeopleCountQuery() : IRequest<ErrorOr<int>>;