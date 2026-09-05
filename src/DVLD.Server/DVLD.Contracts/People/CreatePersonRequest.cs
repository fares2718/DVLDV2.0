namespace DVLD.Contracts.People;

public record CreatePersonRequest(
    string NationalId,
    string FirstName,
    string SecondName,
    string? ThirdName,
    string LastName,
    string MotherName,
    DateOnly DateOfBirth,
    string Phone,
    bool Gender,
    string Email,
    string NationalityCountryCode,
    string? ImagePath,
    string? AltPhone);