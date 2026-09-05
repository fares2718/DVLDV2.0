using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Entities;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Create;

public class CreatePersonCommandHandler
    : IRequestHandler<CreatePersonCommand, ErrorOr<Created>>
{
    private readonly IUnitOfWork _uow;

    public CreatePersonCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<ErrorOr<Created>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        bool uniqueness = await _uow.PersonRepository.IsEmailUnique(request.Email)
                          && await _uow.PersonRepository.IsNationalIdUnique(request.NationalId);

        if (!uniqueness)
            return Error.Validation
                ("Uniqueness.Validation","Email or National ID must be unique");

        var person = Person.Create(request.NationalId,
            request.FirstName,request.SecondName,request.ThirdName,request.LastName,
            request.MotherName,request.DateOfBirth,request.Phone,request.Gender,request.Email,
            request.NationalityCountryCode,request.ImagePath,request.AltPhone);
        
        await _uow.PersonRepository.AddAsync(person, cancellationToken);
        return Result.Created;
    }
}