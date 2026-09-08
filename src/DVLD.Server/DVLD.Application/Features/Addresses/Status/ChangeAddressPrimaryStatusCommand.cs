using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Addresses.Status;

public sealed record ChangeAddressPrimaryStatusCommand(
    Guid AddressId,
    bool IsPrimary
    ):IRequest<ErrorOr<Success>>;