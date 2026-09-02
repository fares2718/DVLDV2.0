using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class Address
{
    public Guid AddressId { get; set; }

    public Guid PersonId { get; set; }

    public byte AddressType { get; set; }

    public string CountryCode { get; set; } = null!;

    public string? Governorate { get; set; }

    public string City { get; set; } = null!;

    public string? Street { get; set; }

    public string? BuildingNumber { get; set; }

    public string? ApartmentNumber { get; set; }

    public string? PostalCode { get; set; }

    public string? AdditionalDetails { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Country CountryCodeNavigation { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;
}
