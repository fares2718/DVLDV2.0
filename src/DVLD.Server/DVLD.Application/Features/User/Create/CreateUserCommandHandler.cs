using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Common;
using DVLD.Domain.Entities;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.User.Create;

public sealed class CreateUserCommandHandler(IUnitOfWork uow, CreateUserValidator validator)
    : IRequestHandler<CreateUserCommand, ErrorOr<Guid>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly CreateUserValidator _validator = validator;

    public async Task<ErrorOr<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Error.Validation("Create.User.Validation", validationResult.Errors.First().ErrorMessage);

        try
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = Domain.Entities.User.Create(request.PersonId, request.Username, passwordHash);
            await _uow.UserRepository.AddAsync(user, cancellationToken);
            var userRole = UserRole.Assign(user.UserId, request.RoleId);
            _uow.UserRepository.AddRoleAsync(userRole);
            await _uow.SaveChangesAsync(cancellationToken);
            return user.UserId;
        }
        catch (DomainException e)
        {
            Console.WriteLine(e);
            return Error.Validation("Domain.Rule.Violation", e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return Error.NotFound("User.NotFound", e.Message);
        }
        catch (Exception e)
        {
            return Error.Failure("Error", e.Message);
        }
    }
}