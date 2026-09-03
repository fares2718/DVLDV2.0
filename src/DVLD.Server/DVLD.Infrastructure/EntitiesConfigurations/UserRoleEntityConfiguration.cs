using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.EntitiesConfigurations;

public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(e => new { e.UserId, e.RoleId });

        builder.HasIndex(e => e.RoleId, "IX_UserRoles_RoleID");

        builder.Property(e => e.UserId).HasColumnName("UserID");
        builder.Property(e => e.RoleId).HasColumnName("RoleID");
        builder.Property(e => e.AssignedAt)
            .HasPrecision(3)
            .IsRequired();

        builder.HasOne<User>().WithMany()
            .HasForeignKey(d => d.AssignedBy)
            .HasConstraintName("FK_UserRoles_AssignedBy");

        builder.HasOne<Role>().WithMany()
            .HasForeignKey(d => d.RoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_UserRoles_Role");

        builder.HasOne<User>().WithMany()
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("FK_UserRoles_User");
    }
}
