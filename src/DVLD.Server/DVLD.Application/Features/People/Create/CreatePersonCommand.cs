using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Create;

public sealed record CreatePersonCommand(
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
    string? ImagePath = null,
    string? AltPhone = null) : IRequest<ErrorOr<Created>>;