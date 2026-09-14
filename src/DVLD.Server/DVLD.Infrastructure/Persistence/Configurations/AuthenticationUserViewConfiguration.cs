using DVLD.Domain.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public sealed class AuthenticationUserViewConfiguration
    : IEntityTypeConfiguration<AuthenticationUserView>
{
    public void Configure(
        EntityTypeBuilder<AuthenticationUserView> builder)
    {
        builder
            .HasNoKey()
            .ToView("vw_AuthenticationUsers");

        builder.Property(x => x.UserId)
            .HasColumnName("UserID");

        builder.Property(x => x.Username)
            .HasColumnName("Username");

        builder.Property(x => x.PasswordHash)
            .HasColumnName("PasswordHash");

        builder.Property(x => x.Roles)
            .HasColumnName("Roles");
    }
}