using System;
using System.Collections.Generic;
using DVLD.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Data;

public partial class DVLDContext : DbContext
{
    public DVLDContext(DbContextOptions<DVLDContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<ApplicationType> ApplicationTypes { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Continent> Continents { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Detention> Detentions { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<InternationalLicense> InternationalLicenses { get; set; }

    public virtual DbSet<License> Licenses { get; set; }

    public virtual DbSet<LicenseClass> LicenseClasses { get; set; }

    public virtual DbSet<LocalDrivingLicenseApplication> LocalDrivingLicenseApplications { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestAppointment> TestAppointments { get; set; }

    public virtual DbSet<TestType> TestTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TR_Addresses_PreventDelete"));

            entity.HasIndex(e => e.PersonId, "IX_Addresses_PersonID");

            entity.HasIndex(e => e.PersonId, "UX_Addresses_Primary")
                .IsUnique()
                .HasFilter("([IsPrimary]=(1))");

            entity.Property(e => e.AddressId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("AddressID");
            entity.Property(e => e.AdditionalDetails).HasMaxLength(500);
            entity.Property(e => e.AddressType).HasDefaultValue((byte)1);
            entity.Property(e => e.ApartmentNumber).HasMaxLength(50);
            entity.Property(e => e.BuildingNumber).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Governorate).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.Street).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasPrecision(3);

            entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.CountryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Addresses_Country");

            entity.HasOne(d => d.Person).WithOne(p => p.Address)
                .HasForeignKey<Address>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Addresses_Person");
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasIndex(e => e.ApplicantPersonId, "IX_Applications_ApplicantPersonID");

            entity.HasIndex(e => e.ApplicationTypeId, "IX_Applications_ApplicationTypeID");

            entity.HasIndex(e => e.CreatedByUserId, "IX_Applications_CreatedByUserID");

            entity.HasIndex(e => e.RelatedApplicationId, "IX_Applications_RelatedApplicationID");

            entity.HasIndex(e => e.RelatedLicenseId, "IX_Applications_RelatedLicenseID");

            entity.HasIndex(e => e.Status, "IX_Applications_Status");

            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.ApplicantPersonId).HasColumnName("ApplicantPersonID");
            entity.Property(e => e.ApplicationDate)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ApplicationTypeId).HasColumnName("ApplicationTypeID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.LastStatusDate)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.PaidFees).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RelatedApplicationId).HasColumnName("RelatedApplicationID");
            entity.Property(e => e.RelatedLicenseId).HasColumnName("RelatedLicenseID");
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.UpdatedAt).HasPrecision(3);

            entity.HasOne(d => d.ApplicantPerson).WithMany(p => p.Applications)
                .HasForeignKey(d => d.ApplicantPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Applications_ApplicantPerson");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.Applications)
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Applications_ApplicationType");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Applications)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Applications_CreatedByUser");

            entity.HasOne(d => d.RelatedApplication).WithMany(p => p.InverseRelatedApplication)
                .HasForeignKey(d => d.RelatedApplicationId)
                .HasConstraintName("FK_Applications_RelatedApplication");

            entity.HasOne(d => d.RelatedLicense).WithMany(p => p.Applications)
                .HasForeignKey(d => d.RelatedLicenseId)
                .HasConstraintName("FK_Applications_RelatedLicense");
        });

        modelBuilder.Entity<ApplicationType>(entity =>
        {
            entity.HasIndex(e => e.Title, "UQ_ApplicationTypes_Title").IsUnique();

            entity.Property(e => e.ApplicationTypeId).HasColumnName("ApplicationTypeID");
            entity.Property(e => e.BaseFees).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.UpdatedAt).HasPrecision(3);
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.EntityId)
                .HasMaxLength(100)
                .HasColumnName("EntityID");
            entity.Property(e => e.EntityName).HasMaxLength(128);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("IPAddress");
            entity.Property(e => e.MachineName).HasMaxLength(255);
            entity.Property(e => e.Timestamp)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AuditLogs_User");
        });

        modelBuilder.Entity<Continent>(entity =>
        {
            entity.HasKey(e => e.ContinentCode).HasName("PK__Continen__9F9AF7B0A1F9EBA2");

            entity.Property(e => e.ContinentCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ContinentName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryCode).HasName("PK__Countrie__5D9B0D2D51CDAF78");

            entity.HasIndex(e => e.ContinentCode, "IX_Countries_Continent_Code");

            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ContinentCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CountryFullName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CountryName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CountryNumber)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Iso3)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("iso3");

            entity.HasOne(d => d.ContinentCodeNavigation).WithMany(p => p.Countries)
                .HasForeignKey(d => d.ContinentCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Countries_Continents");
        });

        modelBuilder.Entity<Detention>(entity =>
        {
            entity.HasIndex(e => e.CreatedByUserId, "IX_Detentions_CreatedByUserID");

            entity.HasIndex(e => e.IsReleased, "IX_Detentions_IsReleased");

            entity.HasIndex(e => e.LicenseId, "IX_Detentions_LicenseID");

            entity.HasIndex(e => e.ReleaseApplicationId, "IX_Detentions_ReleaseApplicationID");

            entity.HasIndex(e => e.ReleasedByUserId, "IX_Detentions_ReleasedByUserID");

            entity.Property(e => e.DetentionId).HasColumnName("DetentionID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.DetainDate)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.FineFees).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LicenseId).HasColumnName("LicenseID");
            entity.Property(e => e.ReleaseApplicationId).HasColumnName("ReleaseApplicationID");
            entity.Property(e => e.ReleaseDate).HasPrecision(3);
            entity.Property(e => e.ReleasedByUserId).HasColumnName("ReleasedByUserID");
            entity.Property(e => e.UpdatedAt).HasPrecision(3);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.DetentionCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detentions_CreatedByUser");

            entity.HasOne(d => d.License).WithMany(p => p.Detentions)
                .HasForeignKey(d => d.LicenseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detentions_License");

            entity.HasOne(d => d.ReleaseApplication).WithMany(p => p.Detentions)
                .HasForeignKey(d => d.ReleaseApplicationId)
                .HasConstraintName("FK_Detentions_ReleaseApplication");

            entity.HasOne(d => d.ReleasedByUser).WithMany(p => p.DetentionReleasedByUsers)
                .HasForeignKey(d => d.ReleasedByUserId)
                .HasConstraintName("FK_Detentions_ReleasedByUser");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TR_Drivers_PreventDelete"));

            entity.HasIndex(e => e.PersonId, "UQ_Drivers_PersonID").IsUnique();

            entity.Property(e => e.DriverId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("DriverID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.UpdatedAt).HasPrecision(3);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Driver_CreatedByUser");

            entity.HasOne(d => d.Person).WithOne(p => p.Driver)
                .HasForeignKey<Driver>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Drivers_Person");
        });

        modelBuilder.Entity<InternationalLicense>(entity =>
        {
            entity.HasIndex(e => e.ApplicationId, "IX_InternationalLicenses_ApplicationID");

            entity.HasIndex(e => e.DriverId, "IX_InternationalLicenses_DriverID");

            entity.HasIndex(e => e.IssuedUsingLocalLicenseId, "IX_InternationalLicenses_LocalLicenseID");

            entity.Property(e => e.InternationalLicenseId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("InternationalLicenseID");
            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DriverId).HasColumnName("DriverID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IssuedUsingLocalLicenseId).HasColumnName("IssuedUsingLocalLicenseID");
            entity.Property(e => e.UpdatedAt).HasPrecision(3);

            entity.HasOne(d => d.Application).WithMany(p => p.InternationalLicenses)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InternationalLicenses_Application");

            entity.HasOne(d => d.Driver).WithMany(p => p.InternationalLicenses)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InternationalLicenses_Driver");

            entity.HasOne(d => d.IssuedUsingLocalLicense).WithMany(p => p.InternationalLicenses)
                .HasForeignKey(d => d.IssuedUsingLocalLicenseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InternationalLicenses_LocalLicense");
        });

        modelBuilder.Entity<License>(entity =>
        {
            entity.HasIndex(e => e.ApplicationId, "IX_Licenses_ApplicationID");

            entity.HasIndex(e => e.DriverId, "IX_Licenses_DriverID");

            entity.HasIndex(e => e.ExpirationDate, "IX_Licenses_ExpirationDate");

            entity.HasIndex(e => e.LicenseClassId, "IX_Licenses_LicenseClassID");

            entity.Property(e => e.LicenseId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("LicenseID");
            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.DriverId).HasColumnName("DriverID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LicenseClassId).HasColumnName("LicenseClassID");
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.PaidFees).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasPrecision(3);

            entity.HasOne(d => d.Application).WithMany(p => p.Licenses)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Licenses_Application");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Licenses)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_License_CreatedByUser");

            entity.HasOne(d => d.Driver).WithMany(p => p.Licenses)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Licenses_Driver");

            entity.HasOne(d => d.LicenseClass).WithMany(p => p.Licenses)
                .HasForeignKey(d => d.LicenseClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Licenses_LicenseClass");
        });

        modelBuilder.Entity<LicenseClass>(entity =>
        {
            entity.HasIndex(e => e.ParentClassId, "IX_LicenseClasses_ParentClassID");

            entity.HasIndex(e => e.ClassName, "UQ_LicenseClasses_ClassName").IsUnique();

            entity.Property(e => e.LicenseClassId).HasColumnName("LicenseClassID");
            entity.Property(e => e.ClassDescription).HasMaxLength(500);
            entity.Property(e => e.ClassFees).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClassName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ParentClassId).HasColumnName("ParentClassID");
            entity.Property(e => e.UpdatedAt).HasPrecision(3);

            entity.HasOne(d => d.ParentClass).WithMany(p => p.InverseParentClass)
                .HasForeignKey(d => d.ParentClassId)
                .HasConstraintName("FK_LicenseClasses_Parent");
        });

        modelBuilder.Entity<LocalDrivingLicenseApplication>(entity =>
        {
            entity.HasIndex(e => e.ApplicationId, "UQ_LocalDrivingLicenseApplications_ApplicationID").IsUnique();

            entity.Property(e => e.LocalDrivingLicenseApplicationId).HasColumnName("LocalDrivingLicenseApplicationID");
            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.LicenseClassId).HasColumnName("LicenseClassID");

            entity.HasOne(d => d.Application).WithOne(p => p.LocalDrivingLicenseApplication)
                .HasForeignKey<LocalDrivingLicenseApplication>(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LocalDrivingLicenseApplications_Application");

            entity.HasOne(d => d.LicenseClass).WithMany(p => p.LocalDrivingLicenseApplications)
                .HasForeignKey(d => d.LicenseClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LocalDrivingLicenseApplications_LicenseClass");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TR_People_PreventDelete"));

            entity.HasIndex(e => e.Email, "IX_People_Email");

            entity.HasIndex(e => new { e.LastName, e.FirstName, e.SecondName, e.ThirdName }, "IX_People_Name");

            entity.HasIndex(e => e.Phone, "IX_People_Phone");

            entity.HasIndex(e => e.NationalId, "UQ__People__NationalID").IsUnique();

            entity.Property(e => e.PersonId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("PersonID");
            entity.Property(e => e.AltPhone).HasMaxLength(30);
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(254);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.ImagePath).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MotherName).HasMaxLength(200);
            entity.Property(e => e.NationalId)
                .HasMaxLength(50)
                .HasColumnName("NationalID");
            entity.Property(e => e.NationalityCountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Phone).HasMaxLength(30);
            entity.Property(e => e.SecondName).HasMaxLength(100);
            entity.Property(e => e.ThirdName).HasMaxLength(100);

            entity.HasOne(d => d.NationalityCountryCodeNavigation).WithMany(p => p.People)
                .HasForeignKey(d => d.NationalityCountryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_People_Countries");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.Name, "UQ_Roles_Name").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasPrecision(3);
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasIndex(e => e.TestAppointmentId, "UQ_Tests_TestAppointmentID").IsUnique();

            entity.Property(e => e.TestId).HasColumnName("TestID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.TestAppointmentId).HasColumnName("TestAppointmentID");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Tests)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tests_CreatedByUser");

            entity.HasOne(d => d.TestAppointment).WithOne(p => p.Test)
                .HasForeignKey<Test>(d => d.TestAppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tests_TestAppointment");
        });

        modelBuilder.Entity<TestAppointment>(entity =>
        {
            entity.HasIndex(e => e.LocalDrivingLicenseApplicationId, "IX_TestAppointments_ApplicationID");

            entity.HasIndex(e => e.AppointmentDate, "IX_TestAppointments_AppointmentDate");

            entity.HasIndex(e => e.CreatedByUserId, "IX_TestAppointments_CreatedByUserID");

            entity.HasIndex(e => e.TestTypeId, "IX_TestAppointments_TestTypeID");

            entity.Property(e => e.TestAppointmentId).HasColumnName("TestAppointmentID");
            entity.Property(e => e.AppointmentDate).HasPrecision(3);
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.LocalDrivingLicenseApplicationId).HasColumnName("LocalDrivingLicenseApplicationID");
            entity.Property(e => e.PaidFees).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TestTypeId).HasColumnName("TestTypeID");
            entity.Property(e => e.UpdatedAt).HasPrecision(3);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.TestAppointments)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestAppointments_CreatedByUser");

            entity.HasOne(d => d.LocalDrivingLicenseApplication).WithMany(p => p.TestAppointments)
                .HasForeignKey(d => d.LocalDrivingLicenseApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestAppointment_LocalLicenseApplication");

            entity.HasOne(d => d.TestType).WithMany(p => p.TestAppointments)
                .HasForeignKey(d => d.TestTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestAppointments_TestType");
        });

        modelBuilder.Entity<TestType>(entity =>
        {
            entity.HasIndex(e => e.OrderInSequence, "UQ_TestTypes_OrderInSequence").IsUnique();

            entity.HasIndex(e => e.Title, "UQ_TestTypes_Title").IsUnique();

            entity.Property(e => e.TestTypeId).HasColumnName("TestTypeID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Fees).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.UpdatedAt).HasPrecision(3);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TR_Users_PreventDelete"));

            entity.HasIndex(e => e.PersonId, "UQ_Users_PersonID").IsUnique();

            entity.HasIndex(e => e.Username, "UQ_Users_Username").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastLoginAt).HasPrecision(3);
            entity.Property(e => e.LockedUntil).HasPrecision(3);
            entity.Property(e => e.PasswordChangedAt).HasPrecision(3);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.UpdatedAt).HasPrecision(3);
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.Person).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Person");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.HasIndex(e => e.RoleId, "IX_UserRoles_RoleID");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.AssignedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.AssignedByNavigation).WithMany(p => p.UserRoleAssignedByNavigations)
                .HasForeignKey(d => d.AssignedBy)
                .HasConstraintName("FK_UserRoles_AssignedBy");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoleUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserRoles_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
