using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class ApplicationType
{
    public int ApplicationTypeId { get; set; }

    public string Title { get; set; } = null!;

    public decimal BaseFees { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
