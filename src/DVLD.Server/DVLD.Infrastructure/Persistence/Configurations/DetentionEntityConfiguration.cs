using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public class DetentionEntityConfiguration : IEntityTypeConfiguration<Detention>
{
    public void Configure(EntityTypeBuilder<Detention> builder)
    {
        builder.HasIndex(e => e.CreatedByUserId, "IX_Detentions_CreatedByUserID");

        builder.HasIndex(e => e.IsReleased, "IX_Detentions_IsReleased");

        builder.HasIndex(e => e.LicenseId, "IX_Detentions_LicenseID");

        builder.HasIndex(e => e.ReleaseApplicationId, "IX_Detentions_ReleaseApplicationID");

        builder.HasIndex(e => e.ReleasedByUserId, "IX_Detentions_ReleasedByUserID");
        
        builder.HasKey(e => e.DetentionId);

        builder.Property(e => e.DetentionId)
            .HasColumnName("DetentionID")
            .UseIdentityColumn(1);
        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .IsRequired();
        builder.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
        builder.Property(e => e.DetainDate)
            .HasPrecision(3)
            .IsRequired();
        builder.Property(e => e.FineFees).HasColumnType("decimal(18, 2)");
        builder.Property(e => e.LicenseId).HasColumnName("LicenseID");
        builder.Property(e => e.ReleaseApplicationId).HasColumnName("ReleaseApplicationID");
        builder.Property(e => e.ReleaseDate).HasPrecision(3);
        builder.Property(e => e.ReleasedByUserId).HasColumnName("ReleasedByUserID");
        builder.Property(e => e.UpdatedAt).HasPrecision(3);

        builder.HasOne<User>().WithMany()
            .HasForeignKey(d => d.CreatedByUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Detentions_CreatedByUser");

        builder.HasOne<License>().WithMany()
            .HasForeignKey(d => d.LicenseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Detentions_License");

        builder.HasOne<Domain.Entities.Application>().WithMany()
            .HasForeignKey(d => d.ReleaseApplicationId)
            .HasConstraintName("FK_Detentions_ReleaseApplication");

        builder.HasOne<User>().WithMany()
            .HasForeignKey(d => d.ReleasedByUserId)
            .HasConstraintName("FK_Detentions_ReleasedByUser");
    }
}
