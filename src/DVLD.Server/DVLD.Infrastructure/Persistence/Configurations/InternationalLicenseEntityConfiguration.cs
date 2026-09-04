using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public class InternationalLicenseEntityConfiguration : IEntityTypeConfiguration<InternationalLicense>
{
    public void Configure(EntityTypeBuilder<InternationalLicense> builder)
    {
        builder.HasIndex(e => e.ApplicationId, "IX_InternationalLicenses_ApplicationID");

        builder.HasIndex(e => e.DriverId, "IX_InternationalLicenses_DriverID");

        builder.HasIndex(e => e.IssuedUsingLocalLicenseId, "IX_InternationalLicenses_LocalLicenseID");

        builder.Property(e => e.InternationalLicenseId)
            .ValueGeneratedNever()
            .HasColumnName("InternationalLicenseID");
        builder.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .IsRequired();
        builder.Property(e => e.DriverId).HasColumnName("DriverID");
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.IssuedUsingLocalLicenseId).HasColumnName("IssuedUsingLocalLicenseID");
        builder.Property(e => e.UpdatedAt).HasPrecision(3);

        builder.HasOne<Domain.Entities.Application>().WithMany()
            .HasForeignKey(d => d.ApplicationId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_InternationalLicenses_Application");

        builder.HasOne<Driver>().WithMany()
            .HasForeignKey(d => d.DriverId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_InternationalLicenses_Driver");

        builder.HasOne<License>().WithMany()
            .HasForeignKey(d => d.IssuedUsingLocalLicenseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_InternationalLicenses_LocalLicense");
    }
}
