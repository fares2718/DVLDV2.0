using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.EntitiesConfigurations;

public class ApplicationTypeEntityConfiguration : IEntityTypeConfiguration<ApplicationType>
{
    public void Configure(EntityTypeBuilder<ApplicationType> builder)
    {
        builder.HasIndex(e => e.Title, "UQ_ApplicationTypes_Title").IsUnique();
        
        builder.HasKey(e => e.ApplicationTypeId);

        builder.Property(e => e.ApplicationTypeId)
            .HasColumnName("ApplicationTypeID")
            .UseIdentityColumn(1);
        builder.Property(e => e.BaseFees)
            .HasColumnType("decimal(18, 2)");
        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .IsRequired();
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.IsActive).IsRequired();
        builder.Property(e => e.Title).HasMaxLength(150);
        builder.Property(e => e.UpdatedAt).HasPrecision(3);
    }
}
