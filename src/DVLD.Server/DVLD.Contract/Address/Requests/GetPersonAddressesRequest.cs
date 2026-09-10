namespace DVLD.Contract.Address.Requests;

public record GetPersonAddressesRequest(
    bool? IsPrimary,
    bool? IsActive,
    byte? AddressType = null);