using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public enum AuditActionType : byte
{
    Create = 1,
    Update = 2,
    Delete = 3,
    Login = 4,
    Logout = 5,
}

public class AuditLog
{
    public long LogId { get; private set; }

    public Guid? UserId { get; private set; }

    public AuditActionType ActionType { get; private set; }

    public string EntityName { get; private set; } = null!;

    public string? EntityId { get; private set; }

    public string? OldValues { get; private set; }

    public string? NewValues { get; private set; }

    public DateTime Timestamp { get; private set; }

    public string? Ipaddress { get; private set; }

    public string? MachineName { get; private set; }

    // For EF Core
    private AuditLog()
    {
    }

    private AuditLog(
        Guid? userId,
        AuditActionType actionType,
        string entityName,
        string? entityId,
        string? oldValues,
        string? newValues,
        string? ipaddress,
        string? machineName)
    {
        UserId = userId;
        ActionType = actionType;
        EntityName = entityName;
        EntityId = entityId;
        OldValues = oldValues;
        NewValues = newValues;
        Ipaddress = ipaddress;
        MachineName = machineName;
        Timestamp = DateTime.UtcNow;
    }

    public static AuditLog Create(
        Guid? userId,
        AuditActionType actionType,
        string entityName,
        string? entityId = null,
        string? oldValues = null,
        string? newValues = null,
        string? ipaddress = null,
        string? machineName = null)
    {
        if (!Enum.IsDefined(actionType))
            throw new DomainException(
                $"Invalid Audit Action Type: {actionType}");

        if (string.IsNullOrWhiteSpace(entityName))
            throw new DomainException("Entity Name cannot be empty");
        
        if (entityName.Length > 100)
            throw new DomainException(
                "Entity Name cannot exceed 100 characters");
        
        return new AuditLog(
            userId,
            actionType,
            entityName,
            entityId,
            oldValues,
            newValues,
            ipaddress,
            machineName);
    }
}