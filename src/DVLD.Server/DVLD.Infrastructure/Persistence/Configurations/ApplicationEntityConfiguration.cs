using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public class ApplicationEntityConfiguration : IEntityTypeConfiguration<Domain.Entities.Application>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Application> builder)
    {
        builder.HasIndex(e => e.ApplicantPersonId, "IX_Applications_ApplicantPersonID");

            builder.HasIndex(e => e.ApplicationTypeId, "IX_Applications_ApplicationTypeID");

            builder.HasIndex(e => e.CreatedByUserId, "IX_Applications_CreatedByUserID");

            builder.HasIndex(e => e.RelatedApplicationId, "IX_Applications_RelatedApplicationID");

            builder.HasIndex(e => e.RelatedLicenseId, "IX_Applications_RelatedLicenseID");

            builder.HasIndex(e => e.Status, "IX_Applications_Status");

            builder.HasKey(e => e.ApplicationId);

            builder.Property(e => e.ApplicationId)
                .HasColumnName("ApplicationID")
                .UseIdentityColumn(1);
            builder.Property(e => e.ApplicantPersonId).HasColumnName("ApplicantPersonID");
            builder.Property(e => e.ApplicationDate)
                .HasPrecision(3)
                .IsRequired();
            builder.Property(e => e.ApplicationTypeId).HasColumnName("ApplicationTypeID");
            builder.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .IsRequired();
            builder.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            builder.Property(e => e.LastStatusDate)
                .HasPrecision(3)
                .IsRequired();
            builder.Property(e => e.PaidFees).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.RelatedApplicationId).HasColumnName("RelatedApplicationID");
            builder.Property(e => e.RelatedLicenseId).HasColumnName("RelatedLicenseID");
            builder.Property(e => e.Status).HasConversion<byte>()
                .HasDefaultValue(ApplicationStatus.New)
                .IsRequired();
            builder.Property(e => e.UpdatedAt).HasPrecision(3);

            builder.HasOne<Person>().WithMany()
                .HasForeignKey(d => d.ApplicantPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Applications_ApplicantPerson");

            builder.HasOne<ApplicationType>().WithMany()
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Applications_ApplicationType");

            builder.HasOne<User>().WithMany()
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Applications_CreatedByUser");

            builder.HasOne<Domain.Entities.Application>().WithMany()
                .HasForeignKey(d => d.RelatedApplicationId)
                .HasConstraintName("FK_Applications_RelatedApplication");

            builder.HasOne<License>().WithMany()
                .HasForeignKey(d => d.RelatedLicenseId)
                .HasConstraintName("FK_Applications_RelatedLicense");
    }
}