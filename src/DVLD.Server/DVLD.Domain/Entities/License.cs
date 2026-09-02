using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class License
{
    public Guid LicenseId { get; set; }

    public Guid DriverId { get; set; }

    public int LicenseClassId { get; set; }

    public DateOnly IssueDate { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public bool IsActive { get; set; }

    public byte IssueReason { get; set; }

    public string? Notes { get; set; }

    public int ApplicationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public decimal PaidFees { get; set; }

    public Guid CreatedByUserId { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual ICollection<Detention> Detentions { get; set; } = new List<Detention>();

    public virtual Driver Driver { get; set; } = null!;

    public virtual ICollection<InternationalLicense> InternationalLicenses { get; set; } = new List<InternationalLicense>();

    public virtual LicenseClass LicenseClass { get; set; } = null!;
}
