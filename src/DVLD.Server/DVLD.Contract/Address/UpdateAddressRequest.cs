namespace DVLD.Contract.Address;

public sealed record UpdateAddressRequest(
    byte AddressType,
    string CountryCode,
    string City,
    string? Governorate,
    string? Street,
    string? BuildingNumber,
    string? ApartmentNumber,
    string? PostalCode,
    string? AdditionalDetails
    );