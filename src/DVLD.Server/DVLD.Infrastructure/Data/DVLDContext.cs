using DVLD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Data;

public partial class DvldContext : DbContext
{
    public DvldContext(DbContextOptions<DvldContext> options)
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DvldContext).Assembly);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
