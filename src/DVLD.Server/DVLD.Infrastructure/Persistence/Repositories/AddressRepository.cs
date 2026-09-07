using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.DTOs;
using DVLD.Domain.Common;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Persistence.Repositories;

public class AddressRepository(DvldContext dvldContext) : IAddressRepository
{
    private readonly DvldContext _dvldContext = dvldContext;

    public async Task AddAsync(Address address, CancellationToken cancellationToken = default)
    {
        if (address.IsPrimary && await PersonHasPrimaryAddress(address.PersonId, cancellationToken))
            throw new DomainException("Person can has only one primary address.");
        
        _dvldContext.Addresses.Add(address);
        await _dvldContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Address> addresses, CancellationToken cancellationToken = default)
    {
        var enumerable = addresses.ToList();
        if(enumerable.Any(a => a.IsPrimary) &&  await PersonHasPrimaryAddress(enumerable.First().PersonId, cancellationToken))
            throw new DomainException("Person can has only one primary address.");
        _dvldContext.Addresses.AddRange(enumerable);
        await _dvldContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangeActivationStatusAsync(Guid addressId, bool isActive, CancellationToken cancellationToken = default)
    {
        var address = await GetByIdAsync(addressId, cancellationToken);
        if (address == null)
            throw new KeyNotFoundException($"Address with Address ID : '{addressId}' was not found");
        if(isActive)
            address.Activate();
        else
        {
            address.Deactivate();
        }
        await _dvldContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangePrimaryStatusAsync(Guid addressId, bool isPrimary, CancellationToken cancellationToken = default)
    {
        var address = await GetByIdAsync(addressId, cancellationToken);
        if (address == null)
            throw new KeyNotFoundException($"Address with Address ID : '{addressId}' was not found");
        if(isPrimary && await PersonHasPrimaryAddress(address.PersonId, cancellationToken))
            throw new DomainException("Person can has only one primary address.");
        if(isPrimary)
            address.SetAsPrimary();
        else
        {
            address.SetAsNotPrimary();
        }

        await _dvldContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Guid addressId, byte addressType, string countryCode, string city, string? governorate, string? street,
        string? buildingNumber, string? apartmentNumber, string? postalCode, string? additionalDetails,
        CancellationToken cancellationToken = default)
    {
        var address = await GetByIdAsync(addressId, cancellationToken);
        if(address == null)
            throw new KeyNotFoundException($"Address with Address ID : '{addressId}' was not found");
        
        address.Update((AddressType)addressType,countryCode, city, governorate,
            street, buildingNumber, apartmentNumber, postalCode, additionalDetails);
        await _dvldContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Address?> GetByIdAsync(Guid addressId, CancellationToken cancellationToken = default)
    {
        var address = await _dvldContext.Addresses.FindAsync(addressId,cancellationToken);
        return address;
    }

    public async Task<IReadOnlyList<AddressDto>> GetPersonAddressesAsync(Guid personId, AddressType? addressType = null, bool? isPrimary = null,
        bool? isActive = null, CancellationToken cancellationToken = default)
    {
        var personAddresses = _dvldContext.Addresses.AsNoTracking()
            .Where(a => a.PersonId == personId);
        
        if(addressType.HasValue)
            personAddresses = personAddresses.Where(a => a.AddressType == addressType.Value);
        if(isPrimary.HasValue)
            personAddresses = personAddresses.Where(a => a.IsPrimary == isPrimary.Value);
        if(isActive.HasValue)
            personAddresses = personAddresses.Where(a => a.IsActive == isActive.Value);
        return await personAddresses.Select(a => 
            new AddressDto(
                a.AddressId,
                a.PersonId,
                a.AddressType.ToString(),
                a.CountryCode,
                a.Governorate,
                a.City,
                a.Street,
                a.BuildingNumber,
                a.ApartmentNumber,
                a.PostalCode,
                a.AdditionalDetails,
                a.IsPrimary,
                a.IsActive,
                a.CreatedAt,
                a.UpdatedAt
                )
        ).ToListAsync(cancellationToken);
    }

    public async Task<bool> PersonHasPrimaryAddress(Guid personId,CancellationToken cancellationToken = default)
    {
        bool result = await _dvldContext.Addresses.Where(a => a.PersonId == personId)
            .AnyAsync(a => a.IsPrimary, cancellationToken: cancellationToken);
        return result;
    }

    public async Task<bool> AddressExists(Guid addressId, CancellationToken cancellationToken = default)
    {
        bool result = await _dvldContext.Addresses.AnyAsync(a => a.AddressId == addressId, cancellationToken: cancellationToken);
        return result;
    }
}