using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;


public sealed class UserRefreshTokenConfiguration
    : IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.ToTable("UserRefreshTokens");


        // Primary Key

        builder.HasKey(x => x.RefreshTokenId);

        builder.Property(x => x.RefreshTokenId)
            .HasColumnName("RefreshTokenID")
            .ValueGeneratedNever();


        // Properties

        builder.Property(x => x.UserId)
            .HasColumnName("UserID")
            .IsRequired();


        builder.Property(x => x.TokenHash)
            .HasMaxLength(500)
            .IsRequired();


        builder.Property(x => x.ExpiresAt)
            .HasColumnType("datetime2(3)")
            .IsRequired();


        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime2(3)")
            .IsRequired();


        builder.Property(x => x.RevokedAt)
            .HasColumnType("datetime2(3)");


        builder.Property(x => x.ReplacedByTokenId);


        builder.Property(x => x.CreatedByIp)
            .HasMaxLength(45);


        builder.Property(x => x.RevokedByIp)
            .HasMaxLength(45);


        builder.Property(x => x.UserAgent)
            .HasMaxLength(500);


        // Relationships

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne<UserRefreshToken>()
            .WithMany()
            .HasForeignKey(x => x.ReplacedByTokenId)
            .OnDelete(DeleteBehavior.Restrict);


        // Indexes

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_UserRefreshTokens_UserID");


        builder.HasIndex(x => x.TokenHash)
            .HasDatabaseName("IX_UserRefreshTokens_TokenHash");
    }
}