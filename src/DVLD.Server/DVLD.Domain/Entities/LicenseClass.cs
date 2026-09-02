using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class LicenseClass
{
    public int LicenseClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public string? ClassDescription { get; set; }

    public byte MinimumAllowedAge { get; set; }

    public byte MaximumAllowedAge { get; set; }

    public byte DefaultValidityYears { get; set; }

    public decimal ClassFees { get; set; }

    public byte PrivilegeLevel { get; set; }

    public int? ParentClassId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte PrivilegeBit { get; set; }

    public virtual ICollection<LicenseClass> InverseParentClass { get; set; } = new List<LicenseClass>();

    public virtual ICollection<License> Licenses { get; set; } = new List<License>();

    public virtual ICollection<LocalDrivingLicenseApplication> LocalDrivingLicenseApplications { get; set; } = new List<LocalDrivingLicenseApplication>();

    public virtual LicenseClass? ParentClass { get; set; }
}
