using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public class LicenseEntityConfiguration : IEntityTypeConfiguration<License>
{
    public void Configure(EntityTypeBuilder<License> builder)
    {
        builder.HasIndex(e => e.ApplicationId, "IX_Licenses_ApplicationID");

        builder.HasIndex(e => e.DriverId, "IX_Licenses_DriverID");

        builder.HasIndex(e => e.ExpirationDate, "IX_Licenses_ExpirationDate");

        builder.HasIndex(e => e.LicenseClassId, "IX_Licenses_LicenseClassID");

        builder.HasKey(e => e.LicenseId);

        builder.Property(e => e.LicenseId)
            .ValueGeneratedNever()
            .HasColumnName("LicenseID");
        builder.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .IsRequired();
        builder.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
        builder.Property(e => e.DriverId).HasColumnName("DriverID");
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.LicenseClassId).HasColumnName("LicenseClassID");
        builder.Property(e => e.Notes).HasMaxLength(1000);
        builder.Property(e => e.PaidFees).HasColumnType("decimal(18, 2)");
        builder.Property(e => e.UpdatedAt).HasPrecision(3);

        builder.HasOne<LocalDrivingLicenseApplication>().WithOne()
            .HasForeignKey<License>(d => d.ApplicationId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Licenses_Application");

        builder.HasOne<User>().WithMany()
            .HasForeignKey(d => d.CreatedByUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_License_CreatedByUser");

        builder.HasOne<Driver>().WithMany()
            .HasForeignKey(d => d.DriverId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Licenses_Driver");

        builder.HasOne<LicenseClass>().WithMany()
            .HasForeignKey(d => d.LicenseClassId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Licenses_LicenseClass");
    }
}
