using DVLD.Application.Abstractions.Persistence;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Update;

public class UpdatePersonNameCommandHandler(UpdatePersonNameValidator validator, IUnitOfWork uow)
    : IRequestHandler<UpdatePersonNameCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdatePersonNameCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Error.Validation(validationResult.Errors.First().ErrorMessage);
        try
        {
            await uow.PersonRepository.UpdatePersonName(request.PersonId
                , request.FirstName,request.SecondName,request.ThirdName,request.LastName
                , request.MotherName,cancellationToken);
            await uow.SaveChangesAsync(cancellationToken);
            return Result.Updated;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Error.NotFound(e.Message);
        }
    }
}