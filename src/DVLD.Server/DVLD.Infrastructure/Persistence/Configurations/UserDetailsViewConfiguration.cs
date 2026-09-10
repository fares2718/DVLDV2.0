using DVLD.Domain.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public sealed class UserDetailsViewConfiguration
    : IEntityTypeConfiguration<UserDetailsView>
{
    public void Configure(EntityTypeBuilder<UserDetailsView> builder)
    {
        builder.HasNoKey();

        builder.ToView("vw_UserDetails");


        // =====================================================
        // User
        // =====================================================

        builder.Property(x => x.UserId)
            .HasColumnName("UserID");

        builder.Property(x => x.PersonId)
            .HasColumnName("PersonID");

        builder.Property(x => x.Username)
            .HasMaxLength(100);

        builder.Property(x => x.IsUserActive)
            .HasColumnName("IsUserActive");

        builder.Property(x => x.IsLocked)
            .HasColumnName("IsLocked");

        builder.Property(x => x.FailedLoginAttempts)
            .HasColumnName("FailedLoginAttempts");

        builder.Property(x => x.LockedUntil)
            .HasColumnName("LockedUntil")
            .HasColumnType("datetime2(3)");

        builder.Property(x => x.LastLoginAt)
            .HasColumnName("LastLoginAt")
            .HasColumnType("datetime2(3)");

        builder.Property(x => x.PasswordChangedAt)
            .HasColumnName("PasswordChangedAt")
            .HasColumnType("datetime2(3)");

        builder.Property(x => x.UserCreatedAt)
            .HasColumnName("UserCreatedAt")
            .HasColumnType("datetime2(3)");

        builder.Property(x => x.UserUpdatedAt)
            .HasColumnName("UserUpdatedAt")
            .HasColumnType("datetime2(3)");


        // =====================================================
        // Person
        // =====================================================

        builder.Property(x => x.NationalId)
            .HasColumnName("NationalID")
            .HasMaxLength(50);

        builder.Property(x => x.FirstName)
            .HasMaxLength(100);

        builder.Property(x => x.SecondName)
            .HasMaxLength(100);

        builder.Property(x => x.ThirdName)
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .HasMaxLength(100);

        builder.Property(x => x.FullName)
            .HasColumnName("FullName");

        builder.Property(x => x.MotherName)
            .HasMaxLength(200);

        builder.Property(x => x.DateOfBirth)
            .HasColumnType("date");

        builder.Property(x => x.Phone)
            .HasMaxLength(30);

        builder.Property(x => x.AltPhone)
            .HasMaxLength(30);

        builder.Property(x => x.Gender);

        builder.Property(x => x.Email)
            .HasMaxLength(254);

        builder.Property(x => x.NationalityCountryCode)
            .HasMaxLength(2)
            .IsFixedLength();

        builder.Property(x => x.ImagePath)
            .HasMaxLength(500);

        builder.Property(x => x.IsPersonActive)
            .HasColumnName("IsPersonActive");

        builder.Property(x => x.PersonCreatedAt)
            .HasColumnName("PersonCreatedAt")
            .HasColumnType("datetime2(0)");


        // =====================================================
        // Roles
        // =====================================================

        builder.Property(x => x.Roles)
            .HasColumnName("Roles");
    }
}