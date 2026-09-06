using DVLD.Application.Abstractions.Persistence;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.People.Activation;

public class ActivatePersonCommandHandler : IRequestHandler<ActivatePersonCommand,ErrorOr<Success>>
{
    private readonly IUnitOfWork _uow;

    public ActivatePersonCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<ErrorOr<Success>> Handle(ActivatePersonCommand request, CancellationToken cancellationToken)
    {
        if (request.PersonId == Guid.Empty)
            return Error.Validation("Person ID Is Required");
        try
        {
            await _uow.PersonRepository.ActivateAsync(request.PersonId,cancellationToken);
            return Result.Success;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Error.NotFound(e.Message);
        }
    }
}