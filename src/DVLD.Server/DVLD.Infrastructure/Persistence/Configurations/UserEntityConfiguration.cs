using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(tb => tb.HasTrigger("TR_Users_PreventDelete"));

        builder.HasIndex(e => e.PersonId, "UQ_Users_PersonID").IsUnique();

        builder.HasIndex(e => e.Username, "UQ_Users_Username").IsUnique();

        builder.HasKey(e => e.UserId);

        builder.Property(e => e.UserId)
            .ValueGeneratedNever()
            .HasColumnName("UserID");
        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .IsRequired();
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.LastLoginAt).HasPrecision(3);
        builder.Property(e => e.LockedUntil).HasPrecision(3);
        builder.Property(e => e.PasswordChangedAt).HasPrecision(3);
        builder.Property(e => e.PasswordHash).HasMaxLength(500);
        builder.Property(e => e.PersonId).HasColumnName("PersonID");
        builder.Property(e => e.UpdatedAt).HasPrecision(3);
        builder.Property(e => e.Username).HasMaxLength(100);

        builder.HasOne<Person>().WithOne()
            .HasForeignKey<User>(d => d.PersonId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Users_Person");
    }
}
