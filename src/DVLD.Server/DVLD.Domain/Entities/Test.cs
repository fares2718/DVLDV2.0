using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class Test
{
    public int TestId { get; set; }

    public int TestAppointmentId { get; set; }

    public bool TestResult { get; set; }

    public string? Notes { get; set; }

    public Guid CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual TestAppointment TestAppointment { get; set; } = null!;
}
