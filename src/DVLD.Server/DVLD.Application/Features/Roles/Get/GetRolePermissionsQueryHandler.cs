using DVLD.Application.Abstractions.Persistence;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Roles.Get;

public class GetRolePermissionsQueryHandler(IUnitOfWork uow) : IRequestHandler<GetRolePermissionsQuery, ErrorOr<long>>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<long>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
    {
        if (request.UserRoles.Count == 0)
            return Error.Validation("Validation.Error","Roles are required");
        try
        {
            long permissions = await _uow.RoleRepository.GetRolePermissions(request.UserRoles);
            return permissions;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("Error", e.Message);
        }
    }
}