using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public class Person
{
    public Guid PersonId { get; private set; }

    public string NationalId { get; private set; } = null!;

    public string FirstName { get; private set; } = null!;

    public string SecondName { get; private set; } = null!;

    public string? ThirdName { get; private set; }

    public string LastName { get; private set; } = null!;

    public string MotherName { get; private set; } = null!;

    public DateOnly DateOfBirth { get; private set; }

    public string Phone { get; private set; } = null!;

    public bool Gender { get; private set; }

    public string Email { get; private set; } = null!;

    public string NationalityCountryCode { get; private set; } = null!;

    public string? ImagePath { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public bool IsActive { get; private set; }

    public string? AltPhone { get; private set; }

    // For EF Core
    private Person()
    {
    }

    private Person(
        string nationalId,
        string firstName,
        string secondName,
        string? thirdName,
        string lastName,
        string motherName,
        DateOnly dateOfBirth,
        string phone,
        bool gender,
        string email,
        string nationalityCountryCode,
        string? imagePath,
        string? altPhone)
    {
        PersonId = Guid.NewGuid();

        NationalId = nationalId;
        FirstName = firstName;
        SecondName = secondName;
        ThirdName = thirdName;
        LastName = lastName;
        MotherName = motherName;
        DateOfBirth = dateOfBirth;
        Phone = phone;
        Gender = gender;
        Email = email;
        NationalityCountryCode = nationalityCountryCode;
        ImagePath = imagePath;
        AltPhone = altPhone;

        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public static Person Create(
        string nationalId,
        string firstName,
        string secondName,
        string? thirdName,
        string lastName,
        string motherName,
        DateOnly dateOfBirth,
        string phone,
        bool gender,
        string email,
        string nationalityCountryCode,
        string? imagePath = null,
        string? altPhone = null)
    {
        ValidateRequiredText(nationalId, "National ID");
        ValidateRequiredText(firstName, "First Name");
        ValidateRequiredText(secondName, "Second Name");
        ValidateRequiredText(lastName, "Last Name");
        ValidateRequiredText(motherName, "Mother Name");
        ValidateRequiredText(phone, "Phone");
        ValidateRequiredText(email, "Email");
        ValidateRequiredText(
            nationalityCountryCode,
            "Nationality Country Code");

        ValidateDateOfBirth(dateOfBirth);

        return new Person(
            nationalId.Trim(),
            firstName.Trim(),
            secondName.Trim(),
            NormalizeOptional(thirdName),
            lastName.Trim(),
            motherName.Trim(),
            dateOfBirth,
            phone.Trim(),
            gender,
            email.Trim(),
            nationalityCountryCode.Trim(),
            NormalizeOptional(imagePath),
            NormalizeOptional(altPhone));
    }

    public void UpdateName(
        string firstName,
        string secondName,
        string? thirdName,
        string lastName,
        string motherName)
    {
        ValidateRequiredText(firstName, "First Name");
        ValidateRequiredText(secondName, "Second Name");
        ValidateRequiredText(lastName, "Last Name");
        ValidateRequiredText(motherName, "Mother Name");

        FirstName = firstName.Trim();
        SecondName = secondName.Trim();
        ThirdName = NormalizeOptional(thirdName);
        LastName = lastName.Trim();
        MotherName = motherName.Trim();
    }

    public void UpdateContactInfo(
        string phone,
        string email,
        string? altPhone)
    {
        ValidateRequiredText(phone, "Phone");
        ValidateRequiredText(email, "Email");

        Phone = phone.Trim();
        Email = email.Trim();
        AltPhone = NormalizeOptional(altPhone);
    }

    public void UpdatePersonalInfo(
        DateOnly dateOfBirth,
        bool gender,
        string nationalityCountryCode)
    {
        ValidateDateOfBirth(dateOfBirth);

        ValidateRequiredText(
            nationalityCountryCode,
            "Nationality Country Code");

        DateOfBirth = dateOfBirth;
        Gender = gender;
        NationalityCountryCode = nationalityCountryCode.Trim();
    }

    public void UpdateImage(string? imagePath)
    {
        ImagePath = NormalizeOptional(imagePath);
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException(
                "Person is already inactive");

        IsActive = false;
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException(
                "Person is already active");

        IsActive = true;
    }

    private static void ValidateRequiredText(
        string value,
        string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(
                $"{fieldName} cannot be empty");
    }

    private static void ValidateDateOfBirth(
        DateOnly dateOfBirth)
    {
        if (dateOfBirth >= DateOnly.FromDateTime(DateTime.UtcNow))
            throw new DomainException(
                "Date of Birth must be in the past");
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}