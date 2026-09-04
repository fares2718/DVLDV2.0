using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public class AddressEntityConfiguration
    : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable(
            "Addresses",
            tb => tb.HasTrigger("TR_Addresses_PreventDelete"));

        // Primary Key
        builder.HasKey(e => e.AddressId);

        builder.Property(e => e.AddressId)
            .HasColumnName("AddressID")
            .ValueGeneratedNever();

        // Person
        builder.Property(e => e.PersonId)
            .HasColumnName("PersonID")
            .IsRequired();

        // Address Type
        builder.Property(e => e.AddressType)
            .HasConversion<byte>()
            .HasDefaultValue((byte)1)
            .IsRequired();

        // Country
        builder.Property(e => e.CountryCode)
            .HasMaxLength(2)
            .IsUnicode(false)
            .IsFixedLength()
            .IsRequired();

        // Location
        builder.Property(e => e.Governorate)
            .HasMaxLength(100);

        builder.Property(e => e.City)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Street)
            .HasMaxLength(200);

        builder.Property(e => e.BuildingNumber)
            .HasMaxLength(50);

        builder.Property(e => e.ApartmentNumber)
            .HasMaxLength(50);

        builder.Property(e => e.PostalCode)
            .HasMaxLength(20);

        builder.Property(e => e.AdditionalDetails)
            .HasMaxLength(500);

        // Status
        builder.Property(e => e.IsPrimary)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        // Dates
        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasPrecision(3);

        // Indexes

        builder.HasIndex(e => e.PersonId)
            .HasDatabaseName("IX_Addresses_PersonID");

        builder.HasIndex(e => e.PersonId)
            .HasDatabaseName("UX_Addresses_Primary")
            .IsUnique()
            .HasFilter("([IsPrimary]=(1))");

        // Person → Addresses
        builder.HasOne<Person>()
            .WithMany()
            .HasForeignKey(e => e.PersonId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Addresses_Person");

        // Country → Addresses
        builder.HasOne<Country>()
            .WithMany()
            .HasForeignKey(e => e.CountryCode)
            .HasPrincipalKey(e => e.CountryCode)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Addresses_Country");
    }
}