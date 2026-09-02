using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class Application
{
    public int ApplicationId { get; set; }

    public Guid ApplicantPersonId { get; set; }

    public int ApplicationTypeId { get; set; }

    public DateTime ApplicationDate { get; set; }

    public byte Status { get; set; }

    public decimal PaidFees { get; set; }

    public Guid CreatedByUserId { get; set; }

    public DateTime LastStatusDate { get; set; }

    public Guid? RelatedLicenseId { get; set; }

    public int? RelatedApplicationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Person ApplicantPerson { get; set; } = null!;

    public virtual ApplicationType ApplicationType { get; set; } = null!;

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual ICollection<Detention> Detentions { get; set; } = new List<Detention>();

    public virtual ICollection<InternationalLicense> InternationalLicenses { get; set; } = new List<InternationalLicense>();

    public virtual ICollection<Application> InverseRelatedApplication { get; set; } = new List<Application>();

    public virtual ICollection<License> Licenses { get; set; } = new List<License>();

    public virtual LocalDrivingLicenseApplication? LocalDrivingLicenseApplication { get; set; }

    public virtual Application? RelatedApplication { get; set; }

    public virtual License? RelatedLicense { get; set; }
}
