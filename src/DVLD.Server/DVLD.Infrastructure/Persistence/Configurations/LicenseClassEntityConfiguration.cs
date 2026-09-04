using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public class LicenseClassEntityConfiguration : IEntityTypeConfiguration<LicenseClass>
{
    public void Configure(EntityTypeBuilder<LicenseClass> builder)
    {
        builder.HasIndex(e => e.ParentClassId, "IX_LicenseClasses_ParentClassID");

        builder.HasIndex(e => e.ClassName, "UQ_LicenseClasses_ClassName").IsUnique();

        builder.HasKey(e => e.LicenseClassId);

        builder.Property(e => e.LicenseClassId).HasColumnName("LicenseClassID").UseIdentityColumn(1);
        builder.Property(e => e.ClassDescription).HasMaxLength(500);
        builder.Property(e => e.ClassFees).HasColumnType("decimal(18, 2)");
        builder.Property(e => e.ClassName).HasMaxLength(100);
        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .IsRequired();
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.ParentClassId).HasColumnName("ParentClassID");
        builder.Property(e => e.UpdatedAt).HasPrecision(3);

        builder.HasOne<LicenseClass>().WithMany()
            .HasForeignKey(d => d.ParentClassId)
            .HasConstraintName("FK_LicenseClasses_Parent");
    }
}
