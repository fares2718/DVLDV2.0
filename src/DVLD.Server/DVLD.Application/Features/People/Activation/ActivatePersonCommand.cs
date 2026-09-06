using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Activation;

public sealed record ActivatePersonCommand(Guid PersonId) : IRequest<ErrorOr<Success>>;