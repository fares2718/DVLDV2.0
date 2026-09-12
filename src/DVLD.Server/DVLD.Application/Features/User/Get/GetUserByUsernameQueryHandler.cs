using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Views;
using MediatR;
using ErrorOr;

namespace DVLD.Application.Features.User.Get;

public class GetUserByUsernameQueryHandler(IUnitOfWork uow, GetUserByUsernameQueryValidator validator)
    : IRequestHandler<GetUserByUsernameQuery, ErrorOr<UserDetailsView>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly GetUserByUsernameQueryValidator _validator = validator;

    public async Task<ErrorOr<UserDetailsView>> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Error.Validation(validationResult.Errors.First().ErrorCode,
                validationResult.Errors.First().ErrorMessage);

        try
        {
            var user = await _uow.UserRepository.GetByUsernameAsync(request.Username, cancellationToken);

            if (user is null)
                return Error.NotFound("User.NotFound", $"User with username : {request.Username} was not found.");

            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("Error",e.Message);
        }
    }
}