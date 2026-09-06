using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Update;

public sealed record UpdatePersonNameCommand(
    Guid PersonId,
    string FirstName,
    string SecondName,
    string? ThirdName,
    string LastName,
    string MotherName
    ) : IRequest<ErrorOr<Updated>>;
