using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class InternationalLicense
{
    public Guid InternationalLicenseId { get; set; }

    public Guid DriverId { get; set; }

    public Guid IssuedUsingLocalLicenseId { get; set; }

    public DateOnly IssueDate { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public bool IsActive { get; set; }

    public int ApplicationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual Driver Driver { get; set; } = null!;

    public virtual License IssuedUsingLocalLicense { get; set; } = null!;
}
