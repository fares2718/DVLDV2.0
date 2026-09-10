namespace DVLD.Contract.Address.Requests;

public sealed record AddAddressRequest(
    Guid PersonId,
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