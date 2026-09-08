using DVLD.Domain.Entities;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Addresses.Add;

public sealed record AddAddressesCommand(
    IEnumerable<AddAddressCommand> Addresses
    ) : IRequest<ErrorOr<Success>>;