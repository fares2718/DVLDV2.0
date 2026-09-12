using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Views;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.User.Get;

public sealed class GetUserByIdQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetUserByIdQuery, ErrorOr<UserDetailsView>>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<UserDetailsView>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
            return Error.Validation("Invalid.Data", "User ID is required");
        
        var user = await _uow.UserRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return Error.NotFound($"User with ID {request.UserId} was not found");

        return user;
    }
}