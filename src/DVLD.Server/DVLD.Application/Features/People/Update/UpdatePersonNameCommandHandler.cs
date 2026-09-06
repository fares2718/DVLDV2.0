using DVLD.Application.Abstractions.Persistence;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Update;

public class UpdatePersonNameCommandHandler : IRequestHandler<UpdatePersonNameCommand,ErrorOr<Updated>>
{
    private readonly UpdatePersonNameValidator _validator;
    private readonly IUnitOfWork _uow;

    public UpdatePersonNameCommandHandler(UpdatePersonNameValidator validator, IUnitOfWork uow)
    {
        _validator = validator;
        _uow = uow;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdatePersonNameCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Error.Validation(validationResult.Errors.First().ErrorMessage);
        try
        {
            await _uow.PersonRepository.UpdatePersonName(request.PersonId
                , request.FirstName,request.SecondName,request.ThirdName,request.LastName
                , request.MotherName,cancellationToken);
            return Result.Updated;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Error.NotFound(e.Message);
        }
    }
}