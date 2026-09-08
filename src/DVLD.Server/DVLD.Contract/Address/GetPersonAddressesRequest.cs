namespace DVLD.Contract.Address;

public record GetPersonAddressesRequest(
    bool? IsPrimary,
    bool? IsActive,
    byte? AddressType = null);