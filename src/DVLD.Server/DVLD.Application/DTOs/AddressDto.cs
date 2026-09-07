namespace DVLD.Application.DTOs;

public record AddressDto(
    Guid AddressId,
    Guid PersonId,
    string AddressType,
    string CountryCode,
    string? Governorate,
    string City,
    string? Street,
    string? BuildingNumber,
    string? ApartmentNumber,
    string? PostalCode,
    string? AdditionalDetails,
    bool IsPrimary,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt);