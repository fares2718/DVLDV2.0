using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.DTOs;
using DVLD.Domain.Views;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.User.Get;

public sealed class GetUsersWithFilterQueryHandler(IUnitOfWork uow, GetUsersWithFilterQueryValidator validator)
    : IRequestHandler<GetUsersWithFilterQuery, ErrorOr<PagedList<UserView>>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly GetUsersWithFilterQueryValidator _validator = validator;

    public async Task<ErrorOr<PagedList<UserView>>> Handle(GetUsersWithFilterQuery request, CancellationToken cancellationToken)
    {

        var validationResult = await _validator.ValidateAsync(request,cancellationToken);

        if (!validationResult.IsValid)
            return Error.Validation(validationResult.Errors.First().ErrorCode,
                validationResult.Errors.First().ErrorMessage);

        try
        {
            var usersPage = await _uow.UserRepository.GetUsersAsync(request.Filter,cancellationToken);
            if(usersPage.TotalCount == 0)
                return Error.NotFound("Users.NotFound","No users found");
            return usersPage;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("Error", e.Message);
        }
    }
}