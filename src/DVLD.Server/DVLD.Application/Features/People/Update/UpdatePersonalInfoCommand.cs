using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Update;

public sealed record UpdatePersonalInfoCommand(
    Guid PersonId,
    DateOnly DateOfBirth,
    string NationalityCountryCode
    ) : IRequest<ErrorOr<Updated>>;