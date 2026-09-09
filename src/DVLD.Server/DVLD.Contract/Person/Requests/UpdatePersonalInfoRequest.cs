namespace DVLD.Contract.Person.Requests;

public sealed record UpdatePersonalInfoRequest(
    DateOnly DateOfBirth,
    string NationalityCountryCode
    );