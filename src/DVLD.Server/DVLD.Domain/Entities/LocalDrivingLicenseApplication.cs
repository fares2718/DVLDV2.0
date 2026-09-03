using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public class LocalDrivingLicenseApplication
{
    public int LocalDrivingLicenseApplicationId { get; private set; }

    public int ApplicationId { get; private set; }

    public int LicenseClassId { get; private set; }

    // For EF Core
    private LocalDrivingLicenseApplication()
    {
    }

    private LocalDrivingLicenseApplication(
        int applicationId,
        int licenseClassId)
    {
        ApplicationId = applicationId;
        LicenseClassId = licenseClassId;
    }

    public static LocalDrivingLicenseApplication Create(
        int applicationId,
        int licenseClassId)
    {
        if (applicationId <= 0)
            throw new DomainException(
                "Application ID must be positive");

        if (licenseClassId <= 0)
            throw new DomainException(
                "License Class ID must be positive");

        return new LocalDrivingLicenseApplication(
            applicationId,
            licenseClassId);
    }
}