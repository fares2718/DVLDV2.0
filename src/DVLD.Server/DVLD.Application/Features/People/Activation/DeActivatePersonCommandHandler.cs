using DVLD.Application.Abstractions.Persistence;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Activation;

public class DeActivatePersonCommandHandler(IUnitOfWork uow)
    : IRequestHandler<DeActivatePersonCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(DeActivatePersonCommand request, CancellationToken cancellationToken)
    {
        if (request.PersonId == Guid.Empty)
            return Error.Validation("Person ID Is Required");
        try
        {
            await uow.PersonRepository.DeActivateAsync(request.PersonId,cancellationToken);
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