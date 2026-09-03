using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public class UserRole
{
    public Guid UserId { get; private set; }
    public int RoleId { get; private set; }
    public DateTime AssignedAt { get; private set; }
    public Guid? AssignedBy { get; private set; }

    // EF Core
    private UserRole() { }

    private UserRole(
        Guid userId,
        int roleId,
        Guid? assignedBy)
    {
        UserId = userId;
        RoleId = roleId;
        AssignedBy = assignedBy;
        AssignedAt = DateTime.UtcNow;
    }

    public static UserRole Assign(
        Guid userId,
        int roleId,
        Guid? assignedBy = null)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User ID cannot be empty");

        if (roleId <= 0)
            throw new DomainException("Role ID must be greater than zero");

        if (assignedBy.HasValue && assignedBy.Value == Guid.Empty)
            throw new DomainException("Assigned By cannot be empty");

        return new UserRole(
            userId,
            roleId,
            assignedBy);
    }
}