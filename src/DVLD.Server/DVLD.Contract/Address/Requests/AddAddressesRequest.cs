namespace DVLD.Contract.Address.Requests;

public sealed record AddAddressesRequest(
    IEnumerable<AddAddressRequest> Addresses
    );