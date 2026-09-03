using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public class TestAppointment
{
    public int TestAppointmentId { get; private set; }

    public int LocalDrivingLicenseApplicationId { get; private set; }

    public int TestTypeId { get; private set; }

    public DateTime AppointmentDate { get; private set; }

    public decimal PaidFees { get; private set; }

    public bool IsLocked { get; private set; }

    public Guid CreatedByUserId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    // EF Core
    private TestAppointment() { }

    private TestAppointment(
        int localDrivingLicenseApplicationId,
        int testTypeId,
        DateTime appointmentDate,
        decimal paidFees,
        Guid createdByUserId)
    {
        LocalDrivingLicenseApplicationId = localDrivingLicenseApplicationId;
        TestTypeId = testTypeId;
        AppointmentDate = appointmentDate;
        PaidFees = paidFees;
        CreatedByUserId = createdByUserId;

        IsLocked = false;
        CreatedAt = DateTime.UtcNow;
    }

    public static TestAppointment Create(
        int localDrivingLicenseApplicationId,
        int testTypeId,
        DateTime appointmentDate,
        decimal paidFees,
        Guid createdByUserId)
    {
        if (localDrivingLicenseApplicationId <= 0)
            throw new DomainException(
                "Local Driving License Application ID must be greater than zero");

        if (testTypeId <= 0)
            throw new DomainException(
                "Test Type ID must be greater than zero");

        if (appointmentDate <= DateTime.UtcNow)
            throw new DomainException(
                "Appointment date must be in the future");

        if (paidFees < 0)
            throw new DomainException(
                "Paid fees cannot be negative");

        if (createdByUserId == Guid.Empty)
            throw new DomainException(
                "Created By User ID cannot be empty");

        return new TestAppointment(
            localDrivingLicenseApplicationId,
            testTypeId,
            appointmentDate,
            paidFees,
            createdByUserId);
    }

    public void Reschedule(DateTime appointmentDate)
    {
        if (IsLocked)
            throw new DomainException(
                "Locked test appointment cannot be rescheduled");

        if (appointmentDate <= DateTime.UtcNow)
            throw new DomainException(
                "Appointment date must be in the future");

        AppointmentDate = appointmentDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateFees(decimal paidFees)
    {
        if (IsLocked)
            throw new DomainException(
                "Locked test appointment cannot be modified");

        if (paidFees < 0)
            throw new DomainException(
                "Paid fees cannot be negative");

        PaidFees = paidFees;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Lock()
    {
        if (IsLocked)
            throw new DomainException(
                "Test appointment is already locked");

        IsLocked = true;
        UpdatedAt = DateTime.UtcNow;
    }
}