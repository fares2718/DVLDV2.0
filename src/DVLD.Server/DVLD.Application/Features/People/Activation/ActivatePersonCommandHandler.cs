using DVLD.Application.Abstractions.Persistence;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Activation;

public class ActivatePersonCommandHandler(IUnitOfWork uow) : IRequestHandler<ActivatePersonCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(ActivatePersonCommand request, CancellationToken cancellationToken)
    {
        if (request.PersonId == Guid.Empty)
            return Error.Validation("Person ID Is Required");
        try
        {
            await uow.PersonRepository.ActivateAsync(request.PersonId,cancellationToken);
            await uow.SaveChangesAsync(cancellationToken);
            return Result.Success;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Error.NotFound(e.Message);
        }
    }
}