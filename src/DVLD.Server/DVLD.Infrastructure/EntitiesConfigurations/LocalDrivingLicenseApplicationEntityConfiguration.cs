using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.EntitiesConfigurations;

public class LocalDrivingLicenseApplicationEntityConfiguration : IEntityTypeConfiguration<LocalDrivingLicenseApplication>
{
    public void Configure(EntityTypeBuilder<LocalDrivingLicenseApplication> builder)
    {
        builder.HasIndex(e => e.ApplicationId, "UQ_LocalDrivingLicenseApplications_ApplicationID").IsUnique();
        builder.HasKey(e => e.ApplicationId);

        builder.Property(e => e.LocalDrivingLicenseApplicationId)
            .UseIdentityColumn(1)
            .HasColumnName("LocalDrivingLicenseApplicationID");
        builder.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
        builder.Property(e => e.LicenseClassId).HasColumnName("LicenseClassID");

        builder.HasOne<Application>().WithOne()
            .HasForeignKey<LocalDrivingLicenseApplication>(d => d.ApplicationId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_LocalDrivingLicenseApplications_Application");

        builder.HasOne<LicenseClass>().WithMany()
            .HasForeignKey(d => d.LicenseClassId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_LocalDrivingLicenseApplications_LicenseClass");
    }
}
