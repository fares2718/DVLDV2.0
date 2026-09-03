using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.EntitiesConfigurations;

public class TestEntityConfiguration : IEntityTypeConfiguration<Test>
{
    public void Configure(EntityTypeBuilder<Test> builder)
    {
        builder.HasIndex(e => e.TestAppointmentId, "UQ_Tests_TestAppointmentID").IsUnique();

        builder.HasKey(e => e.TestId);

        builder.Property(e => e.TestId)
            .UseIdentityColumn(1)
            .HasColumnName("TestID");
        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .IsRequired();
        builder.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
        builder.Property(e => e.Notes).HasMaxLength(1000);
        builder.Property(e => e.TestAppointmentId).HasColumnName("TestAppointmentID");

        builder.HasOne<User>().WithMany()
            .HasForeignKey(d => d.CreatedByUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Tests_CreatedByUser");

        builder.HasOne<TestAppointment>().WithOne()
            .HasForeignKey<Test>(d => d.TestAppointmentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Tests_TestAppointment");
    }
}
