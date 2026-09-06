using DVLD.Application.Abstractions.Persistence;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Update;

public sealed class UpdatePersonalInfoCommandHandler : IRequestHandler<UpdatePersonalInfoCommand,ErrorOr<Updated>>
{
    private readonly IUnitOfWork _uow;
    private readonly UpdatePersonalInfoValidator _validator;

    public UpdatePersonalInfoCommandHandler(IUnitOfWork uow, UpdatePersonalInfoValidator validator)
    {
        _uow = uow;
        _validator = validator;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdatePersonalInfoCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Error.Validation("400", validationResult.Errors.First().ErrorMessage);
        try
        {
            await _uow.PersonRepository.UpdatePersonalInfo(request.PersonId,request.DateOfBirth,request.NationalityCountryCode,cancellationToken);
            return Result.Updated;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Error.NotFound("404","Person not found");
        }
    }
}