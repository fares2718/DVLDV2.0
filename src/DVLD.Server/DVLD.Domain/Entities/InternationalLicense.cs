using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public class InternationalLicense
{
    public Guid InternationalLicenseId { get; private set; }

    public Guid DriverId { get; private set; }

    public Guid IssuedUsingLocalLicenseId { get; private set; }

    public DateOnly IssueDate { get; private set; }

    public DateOnly ExpirationDate { get; private set; }

    public bool IsActive { get; private set; }

    public int ApplicationId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    // For EF Core
    private InternationalLicense()
    {
    }

    private InternationalLicense(
        Guid driverId,
        Guid issuedUsingLocalLicenseId,
        DateOnly issueDate,
        DateOnly expirationDate,
        int applicationId)
    {
        InternationalLicenseId = Guid.NewGuid();

        DriverId = driverId;
        IssuedUsingLocalLicenseId = issuedUsingLocalLicenseId;
        IssueDate = issueDate;
        ExpirationDate = expirationDate;
        ApplicationId = applicationId;

        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static InternationalLicense Create(
        Guid driverId,
        Guid issuedUsingLocalLicenseId,
        DateOnly issueDate,
        DateOnly expirationDate,
        int applicationId)
    {
        if (driverId == Guid.Empty)
            throw new DomainException("Driver ID cannot be empty");

        if (issuedUsingLocalLicenseId == Guid.Empty)
            throw new DomainException(
                "Issued Used Local License ID cannot be empty");

        if (applicationId <= 0)
            throw new DomainException(
                "Application ID must be positive");

        if (expirationDate <= issueDate)
            throw new DomainException(
                "Expiration Date must be after Issue Date");

        return new InternationalLicense(
            driverId,
            issuedUsingLocalLicenseId,
            issueDate,
            expirationDate,
            applicationId);
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException(
                "International License is already inactive");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException(
                "International License is already active");

        if (ExpirationDate < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new DomainException(
                "Expired International License cannot be activated");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
