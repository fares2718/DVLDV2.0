using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Common;
using DVLD.Domain.Entities;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Create;

public class CreatePersonCommandHandler
    : IRequestHandler<CreatePersonCommand, ErrorOr<Created>>
{
    private readonly IUnitOfWork _uow;
    private readonly CreatePersonCommandValidator _validator;

    public CreatePersonCommandHandler(IUnitOfWork uow, CreatePersonCommandValidator validator)
    {
        _uow = uow;
        _validator = validator;
    }

    public async Task<ErrorOr<Created>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var validateResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validateResult.IsValid)
            return Error.Validation(validateResult.Errors.First().ErrorMessage);

        var person = Person.Create(request.NationalId,
            request.FirstName,request.SecondName,request.ThirdName,request.LastName,
            request.MotherName,request.DateOfBirth,request.Phone,request.Gender,request.Email,
            request.NationalityCountryCode,request.ImagePath,request.AltPhone);
        
        try
        {
            await _uow.PersonRepository.AddAsync(person, cancellationToken);
            return Result.Created;
        }
        catch (DomainException e)
        {
            Console.WriteLine(e);
            return Error.Validation(e.Message);
        }
    }
}