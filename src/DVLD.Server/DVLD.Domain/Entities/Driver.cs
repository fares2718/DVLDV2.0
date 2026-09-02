using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class Driver
{
    public Guid DriverId { get; set; }

    public Guid PersonId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid CreatedByUserId { get; set; }

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual ICollection<InternationalLicense> InternationalLicenses { get; set; } = new List<InternationalLicense>();

    public virtual ICollection<License> Licenses { get; set; } = new List<License>();

    public virtual Person Person { get; set; } = null!;
}
