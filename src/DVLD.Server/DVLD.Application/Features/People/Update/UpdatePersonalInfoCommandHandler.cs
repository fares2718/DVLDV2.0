using DVLD.Application.Abstractions.Persistence;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Update;

public sealed class UpdatePersonalInfoCommandHandler(IUnitOfWork uow, UpdatePersonalInfoValidator validator)
    : IRequestHandler<UpdatePersonalInfoCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdatePersonalInfoCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Error.Validation("400", validationResult.Errors.First().ErrorMessage);
        try
        {
            await uow.PersonRepository.UpdatePersonalInfo(request.PersonId,request.DateOfBirth,request.NationalityCountryCode,cancellationToken);
            await uow.SaveChangesAsync(cancellationToken);
            return Result.Updated;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Error.NotFound("404","Person not found");
        }
    }
}