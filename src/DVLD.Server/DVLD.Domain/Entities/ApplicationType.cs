using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public class ApplicationType
{
    public int ApplicationTypeId { get; private set; }

    public string Title { get; private set; } = null!;

    public decimal BaseFees { get; private set; }

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    //For EF Core
    private ApplicationType(){}

    public ApplicationType(string title, decimal baseFees, string? description)
    {
        Title = title;
        BaseFees = baseFees;
        Description = description;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static ApplicationType Create(string title, decimal baseFees, string? description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title can't be null or empty");
        if (baseFees < 0)
            throw new DomainException("Base Fees must be positive");
        return new ApplicationType(title, baseFees, description);
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException("Application type is already active");
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException("Application type is already not active");
        IsActive = false;
        CreatedAt = DateTime.UtcNow;
    }
}
