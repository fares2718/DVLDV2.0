using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public enum LicenseIssueReason : byte
{
    NewLocalDrivingLicense = 1,
    RenewDrivingLicense = 2,
    ReplaceLostLicense = 3,
    ReplaceDamagedLicense = 4,
}

public class License
{
    public Guid LicenseId { get; private set; }

    public Guid DriverId { get; private set; }

    public int LicenseClassId { get; private set; }

    public DateOnly IssueDate { get; private set; }

    public DateOnly ExpirationDate { get; private set; }

    public bool IsActive { get; private set; }

    public LicenseIssueReason IssueReason { get; private set; }

    public string? Notes { get; private set; }

    public int ApplicationId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public decimal PaidFees { get; private set; }

    public Guid CreatedByUserId { get; private set; }

    // For EF Core
    private License()
    {
    }

    private License(
        Guid driverId,
        int licenseClassId,
        DateOnly issueDate,
        DateOnly expirationDate,
        LicenseIssueReason issueReason,
        string? notes,
        int applicationId,
        decimal paidFees,
        Guid createdByUserId)
    {
        LicenseId = Guid.NewGuid();

        DriverId = driverId;
        LicenseClassId = licenseClassId;
        IssueDate = issueDate;
        ExpirationDate = expirationDate;
        IssueReason = issueReason;
        Notes = notes;
        ApplicationId = applicationId;
        PaidFees = paidFees;
        CreatedByUserId = createdByUserId;

        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static License Create(
        Guid driverId,
        int licenseClassId,
        DateOnly issueDate,
        DateOnly expirationDate,
        LicenseIssueReason issueReason,
        string? notes,
        int applicationId,
        decimal paidFees,
        Guid createdByUserId)
    {
        if (driverId == Guid.Empty)
            throw new DomainException("Driver ID cannot be empty");

        if (licenseClassId <= 0)
            throw new DomainException(
                "License Class ID must be positive");

        if (applicationId <= 0)
            throw new DomainException(
                "Application ID must be positive");

        if (createdByUserId == Guid.Empty)
            throw new DomainException(
                "Creator User ID cannot be empty");

        if (expirationDate <= issueDate)
            throw new DomainException(
                "Expiration Date must be after Issue Date");

        if (paidFees < 0)
            throw new DomainException(
                "Paid Fees cannot be negative");

        if (!Enum.IsDefined(issueReason))
            throw new DomainException(
                $"Invalid License Issue Reason: {issueReason}");

        return new License(
            driverId,
            licenseClassId,
            issueDate,
            expirationDate,
            issueReason,
            notes,
            applicationId,
            paidFees,
            createdByUserId);
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException(
                "License is already inactive");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException(
                "License is already active");

        if (ExpirationDate < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new DomainException(
                "Expired License cannot be activated");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }
}