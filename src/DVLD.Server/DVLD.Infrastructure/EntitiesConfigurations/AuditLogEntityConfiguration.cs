using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.EntitiesConfigurations;

public class AuditLogEntityConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(e => e.LogId);

        builder.Property(e => e.LogId)
            .HasColumnName("LogID")
            .UseIdentityColumn(1);
        builder.Property(e => e.EntityId)
            .HasMaxLength(100)
            .HasColumnName("EntityID");
        builder.Property(e => e.EntityName).HasMaxLength(128);
        builder.Property(e => e.Ipaddress)
            .HasMaxLength(45)
            .IsUnicode(false)
            .HasColumnName("IPAddress");
        builder.Property(e => e.MachineName).HasMaxLength(255);
        builder.Property(e => e.Timestamp)
            .HasPrecision(3)
            .IsRequired();
        builder.Property(e => e.UserId).HasColumnName("UserID");

        builder.HasOne<User>().WithMany()
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("FK_AuditLogs_User");
    }
}
