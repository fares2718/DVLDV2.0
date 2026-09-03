using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public enum AddressType : byte
{
    Home = 1,
    Work = 2
}

public class Address
{
    public Guid AddressId { get; private set; }

    public Guid PersonId { get; private set; }

    public AddressType AddressType { get; private set; }

    public string CountryCode { get; private set; } = null!;

    public string? Governorate { get; private set; }

    public string City { get; private set; } = null!;

    public string? Street { get; private set; }

    public string? BuildingNumber { get; private set; }

    public string? ApartmentNumber { get; private set; }

    public string? PostalCode { get; private set; }

    public string? AdditionalDetails { get; private set; }

    public bool IsPrimary { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    // EF Core
    private Address()
    {
    }

    private Address(
        Guid personId,
        AddressType addressType,
        string countryCode,
        string city,
        string? governorate,
        string? street,
        string? buildingNumber,
        string? apartmentNumber,
        string? postalCode,
        string? additionalDetails)
    {
        AddressId = Guid.NewGuid();

        PersonId = personId;
        AddressType = addressType;
        CountryCode = countryCode;
        City = city;

        Governorate = governorate;
        Street = street;
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
        PostalCode = postalCode;
        AdditionalDetails = additionalDetails;

        IsActive = true;
        IsPrimary = false;

        CreatedAt = DateTime.UtcNow;
    }

    public static Address Create(
        Guid personId,
        AddressType addressType,
        string countryCode,
        string city,
        string? governorate = null,
        string? street = null,
        string? buildingNumber = null,
        string? apartmentNumber = null,
        string? postalCode = null,
        string? additionalDetails = null)
    {
        if (personId == Guid.Empty)
            throw new DomainException("Person ID is required.");

        if (string.IsNullOrWhiteSpace(countryCode))
            throw new DomainException("Country code is required.");

        if (string.IsNullOrWhiteSpace(city))
            throw new DomainException("City is required.");

        return new Address(
            personId,
            addressType,
            countryCode,
            city,
            governorate,
            street,
            buildingNumber,
            apartmentNumber,
            postalCode,
            additionalDetails);
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException(
                "Address is already active.");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException(
                "Address is already inactive.");

        IsActive = false;
        IsPrimary = false;

        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAsPrimary()
    {
        if (!IsActive)
            throw new DomainException(
                "An inactive address cannot be primary.");

        if (IsPrimary)
            throw new DomainException(
                "Address is already primary.");

        IsPrimary = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAsNotPrimary()
    {
        if (!IsPrimary)
            throw new DomainException(
                "Address is already not primary.");

        IsPrimary = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        AddressType addressType,
        string countryCode,
        string city,
        string? governorate,
        string? street,
        string? buildingNumber,
        string? apartmentNumber,
        string? postalCode,
        string? additionalDetails)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
            throw new DomainException(
                "Country code is required.");

        if (string.IsNullOrWhiteSpace(city))
            throw new DomainException(
                "City is required.");

        AddressType = addressType;
        CountryCode = countryCode;
        City = city;

        Governorate = governorate ?? Governorate;
        Street = street ?? Street;
        BuildingNumber = buildingNumber ?? BuildingNumber;
        ApartmentNumber = apartmentNumber ?? ApartmentNumber;
        PostalCode = postalCode ?? PostalCode;
        AdditionalDetails = additionalDetails ?? AdditionalDetails;

        UpdatedAt = DateTime.UtcNow;
    }
}