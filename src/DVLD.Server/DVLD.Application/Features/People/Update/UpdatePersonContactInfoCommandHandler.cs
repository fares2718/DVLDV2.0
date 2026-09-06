using DVLD.Application.Abstractions.Persistence;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Update;

public sealed class UpdatePersonContactInfoCommandHandler : IRequestHandler<UpdatePersonContactInfoCommand,ErrorOr<Updated>>
{
    private readonly IUnitOfWork _uow;
    private readonly UpdatePersonContactInfoValidator _validator;

    public UpdatePersonContactInfoCommandHandler(UpdatePersonContactInfoValidator validator, IUnitOfWork uow)
    {
        _validator = validator;
        _uow = uow;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdatePersonContactInfoCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Error.Validation("400",validationResult.Errors.First().ErrorMessage);

        try
        {
            await _uow.PersonRepository.UpdatePersonContactInfo(request.PersonId, request.Phone, request.AltPhone,cancellationToken);
            return Result.Updated;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Error.NotFound("404",e.Message);
        }
    }
}