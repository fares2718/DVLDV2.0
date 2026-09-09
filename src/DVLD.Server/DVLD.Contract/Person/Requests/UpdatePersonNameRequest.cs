namespace DVLD.Contract.Person.Requests;

public sealed record UpdatePersonNameRequest(
    string FirstName,
    string SecondName,
    string? ThirdName,
    string LastName,
    string MotherName
    );