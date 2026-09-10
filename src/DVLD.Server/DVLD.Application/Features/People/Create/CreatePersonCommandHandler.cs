using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.Abstractions.Services;
using DVLD.Domain.Common;
using DVLD.Domain.Entities;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Create;

public class CreatePersonCommandHandler(IUnitOfWork uow, CreatePersonCommandValidator validator,IImageService imageService)
    : IRequestHandler<CreatePersonCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var validateResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validateResult.IsValid)
            return Error.Validation(validateResult.Errors.First().ErrorMessage);

        string imagePath = "";
        if (request is { Image: not null, FileName: not null })
        {
            imagePath = await imageService.UploadImage(request.Image, request.FileName);
        }
        
        var person = Person.Create(request.NationalId,
            request.FirstName,request.SecondName,request.ThirdName,request.LastName,
            request.MotherName,request.DateOfBirth,request.Phone,request.Gender,request.Email,
            request.NationalityCountryCode,imagePath,request.AltPhone);
        
        try
        {
            await uow.PersonRepository.AddAsync(person, cancellationToken);
            await uow.SaveChangesAsync(cancellationToken);
            return person.PersonId;
        }
        catch (DomainException e)
        {
            Console.WriteLine(e);
            return Error.Validation(e.Message);
        }
    }
}