using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public class Detention
{
    public int DetentionId { get; private set; }

    public Guid LicenseId { get; private set; }

    public DateTime DetainDate { get; private set; }

    public decimal FineFees { get; private set; }

    public Guid CreatedByUserId { get; private set; }

    public bool IsReleased { get; private set; }

    public DateTime? ReleaseDate { get; private set; }

    public Guid? ReleasedByUserId { get; private set; }

    public int? ReleaseApplicationId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    // For EF Core
    private Detention()
    {
    }

    private Detention(
        Guid licenseId,
        decimal fineFees,
        Guid createdByUserId)
    {
        LicenseId = licenseId;
        FineFees = fineFees;
        CreatedByUserId = createdByUserId;

        DetainDate = DateTime.UtcNow;
        IsReleased = false;
        CreatedAt = DateTime.UtcNow;
    }

    public static Detention Create(
        Guid licenseId,
        decimal fineFees,
        Guid createdByUserId)
    {
        if (licenseId == Guid.Empty)
            throw new DomainException("License ID cannot be empty");

        if (fineFees < 0)
            throw new DomainException("Fine Fees cannot be negative");

        if (createdByUserId == Guid.Empty)
            throw new DomainException("Creator User ID cannot be empty");

        return new Detention(
            licenseId,
            fineFees,
            createdByUserId);
    }

    public void Release(
        Guid releasedByUserId,
        int releaseApplicationId)
    {
        if (IsReleased)
            throw new DomainException("Detention is already released");

        if (releasedByUserId == Guid.Empty)
            throw new DomainException("Released By User ID cannot be empty");

        if (releaseApplicationId <= 0)
            throw new DomainException(
                "Release Application ID must be positive");

        IsReleased = true;
        ReleaseDate = DateTime.UtcNow;
        ReleasedByUserId = releasedByUserId;
        ReleaseApplicationId = releaseApplicationId;
        UpdatedAt = DateTime.UtcNow;
    }
}
