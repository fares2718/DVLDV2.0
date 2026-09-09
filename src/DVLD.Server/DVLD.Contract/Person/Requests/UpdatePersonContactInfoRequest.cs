namespace DVLD.Contract.Person.Requests;

public sealed record UpdatePersonContactInfoRequest(
    string Phone,
    string? AltPhone
    );