namespace DVLD.Domain.Views;

public class PersonSummary
{
    public Guid PersonId { get; private set; }

    public string NationalId { get; private set; } = null!;

    public string FullName { get; private set; } = null!;

    public string MotherName { get; private set; } = null!;

    public DateOnly DateOfBirth { get; private set; }

    public string Phone { get; private set; } = null!;
    public string? AltPhone { get; private set; }

    public string Email { get; private set; } = null!;

    public string Gender { get; private set; } = null!;

    public string NationalityCountryCode { get; private set; } = null!;

    public bool IsActive { get; private set; }
    
    private PersonSummary(){}
}