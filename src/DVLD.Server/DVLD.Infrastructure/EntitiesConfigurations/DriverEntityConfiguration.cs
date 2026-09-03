using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.EntitiesConfigurations;

public class DriverEntityConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable(tb => tb.HasTrigger("TR_Drivers_PreventDelete"));

        builder.HasIndex(e => e.PersonId, "UQ_Drivers_PersonID").IsUnique();
        
        builder.HasKey(e => e.DriverId);

        builder.Property(e => e.DriverId)
            .HasColumnName("DriverID")
            .ValueGeneratedNever();
        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .IsRequired();
        builder.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.PersonId).HasColumnName("PersonID");
        builder.Property(e => e.UpdatedAt).HasPrecision(3);

        builder.HasOne<User>().WithMany()
            .HasForeignKey(d => d.CreatedByUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Driver_CreatedByUser");

        builder.HasOne<Person>().WithOne()
            .HasForeignKey<Driver>(d => d.PersonId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Drivers_Person");
    }
}
