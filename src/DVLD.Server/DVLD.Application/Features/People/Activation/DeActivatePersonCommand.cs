using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Activation;

public sealed record DeActivatePersonCommand (
    Guid PersonId
    ) : IRequest<ErrorOr<Success>>;