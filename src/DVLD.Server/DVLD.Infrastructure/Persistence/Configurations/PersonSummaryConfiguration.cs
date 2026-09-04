using DVLD.Domain.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Infrastructure.Persistence.Configurations;

public class PersonSummaryConfiguration
    : IEntityTypeConfiguration<PersonSummary>
{
    public void Configure(EntityTypeBuilder<PersonSummary> builder)
    {
        builder.HasNoKey();

        builder.ToView("vw_PersonSummary");

        builder.Property(x => x.PersonId)
            .HasColumnName("PersonId");

        builder.Property(x => x.NationalId)
            .HasColumnName("NationalId");

        builder.Property(x => x.FullName)
            .HasColumnName("FullName");
        
        builder.Property(x => x.MotherName)
            .HasColumnName("MotherName");

        builder.Property(x => x.DateOfBirth)
            .HasColumnName("DateOfBirth");

        builder.Property(x => x.Phone)
            .HasColumnName("Phone");
        builder.Property(x => x.AltPhone)
            .HasColumnName("AltPhone");

        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .IsRequired();

        builder.Property(x => x.Gender)
            .HasColumnName("Gender");

        builder.Property(x => x.NationalityCountryCode)
            .HasColumnName("NationalityCountryCode");

        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive");
    }
}