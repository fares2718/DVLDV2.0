using DVLD.Domain.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public sealed class UserViewConfiguration
    : IEntityTypeConfiguration<UserView>
{
    public void Configure(EntityTypeBuilder<UserView> builder)
    {
        builder.HasNoKey();

        builder.ToView("vw_Users");


        builder.Property(x => x.UserId)
            .HasColumnName("UserID");


        builder.Property(x => x.PersonId)
            .HasColumnName("PersonID");


        builder.Property(x => x.Username)
            .HasColumnName("Username")
            .HasMaxLength(100);


        builder.Property(x => x.FullName)
            .HasColumnName("FullName");


        builder.Property(x => x.NationalId)
            .HasColumnName("NationalID")
            .HasMaxLength(50);


        builder.Property(x => x.Phone)
            .HasColumnName("Phone")
            .HasMaxLength(30);


        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .HasMaxLength(254);


        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive");


        builder.Property(x => x.IsLocked)
            .HasColumnName("IsLocked");


        builder.Property(x => x.Roles)
            .HasColumnName("Roles");
    }
}