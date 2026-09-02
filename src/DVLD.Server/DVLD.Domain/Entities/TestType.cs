using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class TestType
{
    public int TestTypeId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Fees { get; set; }

    public byte OrderInSequence { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<TestAppointment> TestAppointments { get; set; } = new List<TestAppointment>();
}
