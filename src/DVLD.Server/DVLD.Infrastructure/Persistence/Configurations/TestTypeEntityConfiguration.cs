using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public class TestTypeEntityConfiguration : IEntityTypeConfiguration<TestType>
{
    public void Configure(EntityTypeBuilder<TestType> builder)
    {
        builder.HasIndex(e => e.OrderInSequence, "UQ_TestTypes_OrderInSequence").IsUnique();

        builder.HasIndex(e => e.Title, "UQ_TestTypes_Title").IsUnique();

        builder.HasKey(e => e.TestTypeId);

        builder.Property(e => e.TestTypeId)
            .UseIdentityColumn(1)
            .HasColumnName("TestTypeID");
        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .IsRequired();
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.Fees).HasColumnType("decimal(18, 2)");
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.Title).HasMaxLength(150);
        builder.Property(e => e.UpdatedAt).HasPrecision(3);
    }
}
