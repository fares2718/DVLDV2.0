using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.EntitiesConfigurations;

public class ContinentEntityConfiguration : IEntityTypeConfiguration<Continent>
{
    public void Configure(EntityTypeBuilder<Continent> builder)
    {
        builder.HasKey(e => e.ContinentCode).HasName("PK_Continents");

        builder.Property(e => e.ContinentCode)
            .HasMaxLength(2)
            .IsUnicode(false)
            .IsFixedLength();
        builder.Property(e => e.ContinentName)
            .HasMaxLength(255)
            .IsUnicode(false);
    }
}
