using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Addresses.Update;

public sealed record UpdateAddressCommand(
    Guid AddressId,
    byte AddressType,
    string CountryCode,
    string City,
    string? Governorate,
    string? Street,
    string? BuildingNumber,
    string? ApartmentNumber,
    string? PostalCode,
    string? AdditionalDetails
    ): IRequest<ErrorOr<Updated>>;