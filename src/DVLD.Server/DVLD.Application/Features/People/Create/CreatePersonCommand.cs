using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

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
    string? FileName = null,
    IFormFile?  Image = null,
    string? AltPhone = null) : IRequest<ErrorOr<Guid>>;