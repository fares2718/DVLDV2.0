using System;
using System.Collections.Generic;

namespace DVLD.Infrastructure;

public partial class AuditLog
{
    public long LogId { get; set; }

    public Guid? UserId { get; set; }

    public byte ActionType { get; set; }

    public string EntityName { get; set; } = null!;

    public string? EntityId { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public DateTime Timestamp { get; set; }

    public string? Ipaddress { get; set; }

    public string? MachineName { get; set; }

    public virtual User? User { get; set; }
}
