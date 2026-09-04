using Microsoft.EntityFrameworkCore;
using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public class PersonEntityConfiguration:IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable(tb => tb.HasTrigger("TR_People_PreventDelete"));

        builder.HasKey(e => e.PersonId);

        builder.HasIndex(e => new { e.LastName, e.FirstName, e.SecondName, e.ThirdName }, "IX_People_Name");

        builder.HasIndex(e => e.Phone, "IX_People_Phone");

        builder.HasIndex(e => e.NationalId, "UQ_People_NationalID").IsUnique();
        builder.HasIndex(e => e.Email, "UQ_People_Email").IsUnique();

        builder.Property(e => e.PersonId)
            .ValueGeneratedNever()
            .HasColumnName("PersonID");
        builder.Property(e => e.AltPhone).HasMaxLength(30);
        builder.Property(e => e.CreatedAt)
            .HasPrecision(0)
            .IsRequired();
        builder.Property(e => e.Email).HasMaxLength(254);
        builder.Property(e => e.FirstName).HasMaxLength(100);
        builder.Property(e => e.ImagePath).HasMaxLength(500);
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.LastName).HasMaxLength(100);
        builder.Property(e => e.MotherName).HasMaxLength(200);
        builder.Property(e => e.NationalId)
            .HasMaxLength(50)
            .HasColumnName("NationalID");
        builder.Property(e => e.NationalityCountryCode)
            .HasMaxLength(2)
            .IsUnicode(false)
            .IsFixedLength();
        builder.Property(e => e.Phone).HasMaxLength(30);
        builder.Property(e => e.SecondName).HasMaxLength(100);
        builder.Property(e => e.ThirdName).HasMaxLength(100);

        builder.HasOne<Country>().WithMany()
            .HasForeignKey(d => d.NationalityCountryCode)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_People_Countries");
    }
}