using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public class TestType
{
    public int TestTypeId { get; private set; }

    public string Title { get; private set; } = null!;

    public string? Description { get; private set; }

    public decimal Fees { get; private set; }

    public byte OrderInSequence { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    // EF Core
    private TestType() { }

    private TestType(
        string title,
        string? description,
        decimal fees,
        byte orderInSequence)
    {
        Title = title;
        Description = NormalizeOptional(description);
        Fees = fees;
        OrderInSequence = orderInSequence;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static TestType Create(
        string title,
        string? description,
        decimal fees,
        byte orderInSequence)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Test Type Title cannot be empty");

        if (fees < 0)
            throw new DomainException("Test Type Fees cannot be negative");

        if (orderInSequence == 0)
            throw new DomainException(
                "Order In Sequence must be greater than zero");

        return new TestType(
            title.Trim(),
            description,
            fees,
            orderInSequence);
    }

    public void UpdateInfo(
        string title,
        string? description,
        byte orderInSequence)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Test Type Title cannot be empty");

        if (orderInSequence == 0)
            throw new DomainException(
                "Order In Sequence must be greater than zero");

        Title = title.Trim();
        Description = NormalizeOptional(description);
        OrderInSequence = orderInSequence;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateFees(decimal fees)
    {
        if (fees < 0)
            throw new DomainException("Test Type Fees cannot be negative");

        Fees = fees;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException("Test type is already active");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException("Test type is already inactive");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}