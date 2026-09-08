using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Addresses.Status;

public sealed record ChangeAddressActivationStatusCommand(
    Guid AddressId,
    bool IsActive
    ):IRequest<ErrorOr<Success>>;