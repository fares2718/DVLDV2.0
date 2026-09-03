using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.EntitiesConfigurations;

public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasIndex(e => e.Name, "UQ_Roles_Name").IsUnique();

        builder.HasKey(e => e.RoleId);

        builder.Property(e => e.RoleId).HasColumnName("RoleID");
        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .IsRequired();
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.Name).HasMaxLength(100);
        builder.Property(e => e.UpdatedAt).HasPrecision(3);
    }
}
