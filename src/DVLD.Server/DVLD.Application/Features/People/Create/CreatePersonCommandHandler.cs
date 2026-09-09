using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Common;
using DVLD.Domain.Entities;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Create;

public class CreatePersonCommandHandler(IUnitOfWork uow, CreatePersonCommandValidator validator)
    : IRequestHandler<CreatePersonCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var validateResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validateResult.IsValid)
            return Error.Validation(validateResult.Errors.First().ErrorMessage);

        var person = Person.Create(request.NationalId,
            request.FirstName,request.SecondName,request.ThirdName,request.LastName,
            request.MotherName,request.DateOfBirth,request.Phone,request.Gender,request.Email,
            request.NationalityCountryCode,request.ImagePath,request.AltPhone);
        
        try
        {
            var newId = await uow.PersonRepository.AddAsync(person, cancellationToken);
            return newId;
        }
        catch (DomainException e)
        {
            Console.WriteLine(e);
            return Error.Validation(e.Message);
        }
    }
}