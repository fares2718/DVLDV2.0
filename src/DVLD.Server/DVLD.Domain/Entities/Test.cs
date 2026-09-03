using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public class Test
{
    public int TestId { get; private set; }

    public int TestAppointmentId { get; private set; }

    public bool TestResult { get; private set; }

    public string? Notes { get; private set; }

    public Guid CreatedByUserId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    // EF Core
    private Test() { }

    private Test(
        int testAppointmentId,
        bool testResult,
        string? notes,
        Guid createdByUserId)
    {
        TestAppointmentId = testAppointmentId;
        TestResult = testResult;
        Notes = NormalizeOptional(notes);
        CreatedByUserId = createdByUserId;
        CreatedAt = DateTime.UtcNow;
    }

    public static Test Create(
        int testAppointmentId,
        bool testResult,
        string? notes,
        Guid createdByUserId)
    {
        if (testAppointmentId <= 0)
            throw new DomainException(
                "Test Appointment ID must be greater than zero");

        if (createdByUserId == Guid.Empty)
            throw new DomainException(
                "Created By User ID cannot be empty");

        return new Test(
            testAppointmentId,
            testResult,
            notes,
            createdByUserId);
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}