using DVLD.Application.DTOs;
using DVLD.Domain.Entities;

namespace DVLD.Application.Abstractions.Persistence;

public interface IAddressRepository
{
    Task AddAsync(
        Address address,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(
        IEnumerable<Address> addresses,
        CancellationToken cancellationToken = default);


    // Status

    Task ChangeActivationStatusAsync(
        Guid addressId,
        bool isActive,
        CancellationToken cancellationToken = default);


    // Primary

    Task ChangePrimaryStatusAsync(
        Guid addressId,
        bool isPrimary,
        CancellationToken cancellationToken = default);


    // Update

    Task UpdateAsync(
        Guid addressId,
        byte addressType,
        string countryCode,
        string city,
        string? governorate,
        string? street,
        string? buildingNumber,
        string? apartmentNumber,
        string? postalCode,
        string? additionalDetails,
        CancellationToken cancellationToken = default);


    // Queries

    
    Task<bool> PersonHasPrimaryAddress(Guid personId,CancellationToken cancellationToken = default);
    Task<bool> AddressExists(Guid addressId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AddressDto>> GetPersonAddressesAsync(
        Guid personId,
        AddressType? addressType = null,
        bool? isPrimary = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default);
}