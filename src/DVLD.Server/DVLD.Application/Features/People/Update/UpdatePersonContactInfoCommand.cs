using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Update;

public sealed record UpdatePersonContactInfoCommand(
    Guid PersonId,
    string Phone,
    string? AltPhone
    ):IRequest<ErrorOr<Updated>>;