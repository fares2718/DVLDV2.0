using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public class Driver
{
    public Guid DriverId { get; private set; }

    public Guid PersonId { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public Guid CreatedByUserId { get; private set; }

    // For EF Core
    private Driver()
    {
    }

    private Driver(
        Guid personId,
        Guid createdByUserId)
    {
        DriverId = Guid.NewGuid();
        PersonId = personId;
        CreatedByUserId = createdByUserId;

        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static Driver Create(
        Guid personId,
        Guid createdByUserId)
    {
        if (personId == Guid.Empty)
            throw new DomainException("Person ID cannot be empty");

        if (createdByUserId == Guid.Empty)
            throw new DomainException("Creator User ID cannot be empty");

        return new Driver(
            personId,
            createdByUserId);
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException("Driver is already inactive");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException("Driver is already active");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}