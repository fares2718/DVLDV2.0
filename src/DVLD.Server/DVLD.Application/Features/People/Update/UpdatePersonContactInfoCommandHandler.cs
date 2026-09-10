using DVLD.Application.Abstractions.Persistence;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Update;

public sealed class UpdatePersonContactInfoCommandHandler(UpdatePersonContactInfoValidator validator, IUnitOfWork uow)
    : IRequestHandler<UpdatePersonContactInfoCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdatePersonContactInfoCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Error.Validation("400",validationResult.Errors.First().ErrorMessage);

        try
        {
            await uow.PersonRepository.UpdatePersonContactInfo(request.PersonId, request.Phone, request.AltPhone,cancellationToken);
            await uow.SaveChangesAsync(cancellationToken);
            return Result.Updated;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Error.NotFound("404",e.Message);
        }
    }
}