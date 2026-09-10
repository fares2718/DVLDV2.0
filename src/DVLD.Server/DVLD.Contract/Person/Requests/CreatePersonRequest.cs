using Microsoft.AspNetCore.Http;

namespace DVLD.Contract.Person.Requests;

public sealed record CreatePersonRequest(
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
    string? FileName = null,
    IFormFile? Image = null,
    string? AltPhone = null
);