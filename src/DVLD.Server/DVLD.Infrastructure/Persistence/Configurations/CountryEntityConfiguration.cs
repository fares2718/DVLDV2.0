using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public class CountryEntityConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(e => e.CountryCode).HasName("PK_Countries");

        builder.HasIndex(e => e.ContinentCode, "IX_Countries_Continent_Code");

        builder.Property(e => e.CountryCode)
            .HasMaxLength(2)
            .IsUnicode(false)
            .IsFixedLength();
        builder.Property(e => e.ContinentCode)
            .HasMaxLength(2)
            .IsUnicode(false)
            .IsFixedLength();
        builder.Property(e => e.CountryFullName)
            .HasMaxLength(255)
            .IsUnicode(false);
        builder.Property(e => e.CountryName)
            .HasMaxLength(255)
            .IsUnicode(false);
        builder.Property(e => e.CountryNumber)
            .HasMaxLength(3)
            .IsUnicode(false)
            .IsFixedLength();
        builder.Property(e => e.Iso3)
            .HasMaxLength(3)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("iso3");

        builder.HasOne<Continent>().WithMany()
            .HasForeignKey(d => d.ContinentCode)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Countries_Continents");
    }
}
