using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class Person
{
    public Guid PersonId { get; set; }

    public string NationalId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public string? ThirdName { get; set; }

    public string LastName { get; set; } = null!;

    public string MotherName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Phone { get; set; } = null!;

    public bool Gender { get; set; }

    public string Email { get; set; } = null!;

    public string NationalityCountryCode { get; set; } = null!;

    public string? ImagePath { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public string? AltPhone { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual Driver? Driver { get; set; }

    public virtual Country NationalityCountryCodeNavigation { get; set; } = null!;

    public virtual User? User { get; set; }
}
