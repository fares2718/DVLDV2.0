using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public class TestAppointmentEntityConfiguration : IEntityTypeConfiguration<TestAppointment>
{
    public void Configure(EntityTypeBuilder<TestAppointment> builder)
    {
        builder.HasIndex(e => e.LocalDrivingLicenseApplicationId, "IX_TestAppointments_ApplicationID");

        builder.HasIndex(e => e.AppointmentDate, "IX_TestAppointments_AppointmentDate");

        builder.HasIndex(e => e.CreatedByUserId, "IX_TestAppointments_CreatedByUserID");

        builder.HasIndex(e => e.TestTypeId, "IX_TestAppointments_TestTypeID");

        builder.HasKey(e => e.TestAppointmentId);

        builder.Property(e => e.TestAppointmentId)
            .UseIdentityColumn(1)
            .HasColumnName("TestAppointmentID");
        builder.Property(e => e.AppointmentDate).HasPrecision(3);
        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .IsRequired();
        builder.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
        builder.Property(e => e.LocalDrivingLicenseApplicationId).HasColumnName("LocalDrivingLicenseApplicationID");
        builder.Property(e => e.PaidFees).HasColumnType("decimal(18, 2)");
        builder.Property(e => e.TestTypeId).HasColumnName("TestTypeID");
        builder.Property(e => e.UpdatedAt).HasPrecision(3);

        builder.HasOne<User>().WithMany()
            .HasForeignKey(d => d.CreatedByUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_TestAppointments_CreatedByUser");

        builder.HasOne<LocalDrivingLicenseApplication>().WithMany()
            .HasForeignKey(d => d.LocalDrivingLicenseApplicationId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_TestAppointment_LocalLicenseApplication");

        builder.HasOne<TestType>().WithMany()
            .HasForeignKey(d => d.TestTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_TestAppointments_TestType");
    }
}
