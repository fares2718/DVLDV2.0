using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class TestAppointment
{
    public int TestAppointmentId { get; set; }

    public int LocalDrivingLicenseApplicationId { get; set; }

    public int TestTypeId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public decimal PaidFees { get; set; }

    public bool IsLocked { get; set; }

    public Guid CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual LocalDrivingLicenseApplication LocalDrivingLicenseApplication { get; set; } = null!;

    public virtual Test? Test { get; set; }

    public virtual TestType TestType { get; set; } = null!;
}
