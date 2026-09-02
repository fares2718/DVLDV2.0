using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class Detention
{
    public int DetentionId { get; set; }

    public Guid LicenseId { get; set; }

    public DateTime DetainDate { get; set; }

    public decimal FineFees { get; set; }

    public Guid CreatedByUserId { get; set; }

    public bool IsReleased { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public Guid? ReleasedByUserId { get; set; }

    public int? ReleaseApplicationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual License License { get; set; } = null!;

    public virtual Application? ReleaseApplication { get; set; }

    public virtual User? ReleasedByUser { get; set; }
}
