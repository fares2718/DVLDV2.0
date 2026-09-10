using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Common;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Addresses.Status;

public sealed class ChangeAddressPrimaryStatusCommandHandler(IUnitOfWork uow)
    : IRequestHandler<ChangeAddressPrimaryStatusCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<Success>> Handle(ChangeAddressPrimaryStatusCommand request, CancellationToken cancellationToken)
    {
        if (request.AddressId == Guid.Empty)
            return Error.Validation("Address.Violation","Address Id is required");
        try
        {
            await _uow.AddressRepository.ChangePrimaryStatusAsync(request.AddressId, request.IsPrimary,
                cancellationToken);
            await uow.SaveChangesAsync(cancellationToken);
            return Result.Success;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Error.NotFound("Address.NotFound", e.Message);
        }
        catch (DomainException e)
        {
            return Error.Validation("Domain.Rule.Violation", e.Message);
        }
        catch (Exception e)
        {
            return Error.Failure("Error", e.Message);
        }
    }
}