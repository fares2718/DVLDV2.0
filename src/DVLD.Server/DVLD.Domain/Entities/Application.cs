using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;


public enum ApplicationStatus : byte
{
    New = 1,
    Completed = 2,
    Cancelled = 3,
}
public class Application
{
    public int ApplicationId { get; private set; }

    public Guid ApplicantPersonId { get; private set; }

    public int ApplicationTypeId { get; private set; }

    public DateTime ApplicationDate { get; private set; }

    public ApplicationStatus Status { get; private set; }

    public decimal PaidFees { get; private set; }

    public Guid CreatedByUserId { get; private set; }

    public DateTime LastStatusDate { get; private set; }

    public Guid? RelatedLicenseId { get; private set; }

    public int? RelatedApplicationId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }
    
    //For EF Core
    private Application(){}

    private Application(Guid applicantPersonId, int applicationTypeId, DateTime applicationDate , decimal paidFees,
        Guid createdByUserId, Guid? relatedLicenseId, int? relatedApplicationId)
    {
        ApplicantPersonId = applicantPersonId;
        ApplicationTypeId = applicationTypeId;
        ApplicationDate = applicationDate;
        PaidFees = paidFees;
        CreatedByUserId = createdByUserId;
        ApplicationDate = DateTime.UtcNow;
        Status = ApplicationStatus.New;
        LastStatusDate = DateTime.UtcNow;
        RelatedLicenseId = relatedLicenseId;
        RelatedApplicationId = relatedApplicationId;
    }

    public static Application Create(Guid applicantPersonId, int applicationTypeId, decimal paidFees,
        Guid createdByUserId, Guid? relatedLicenseId, int? relatedApplicationId,DateTime? applicationDate)
    {
        if (applicantPersonId == Guid.Empty)
            throw new DomainException("Applicant Person ID cannot be empty");
        if (applicationTypeId < 1)
            throw new DomainException("Application Type ID must be positive");
        if (paidFees < 0)
            throw new DomainException("Paid Fees must be positive");
        if(createdByUserId == Guid.Empty)
            throw new DomainException("Creator User ID cannot be empty");
        return new Application(applicantPersonId, applicationTypeId, applicationDate ?? DateTime.UtcNow
            ,paidFees, createdByUserId, relatedLicenseId, relatedApplicationId);
    }

    public void Complete()
    {
        EnsureStatus(ApplicationStatus.New);

        Status = ApplicationStatus.Completed;
        LastStatusDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        EnsureStatus(ApplicationStatus.New);

        Status = ApplicationStatus.Cancelled;
        LastStatusDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    private void EnsureStatus(ApplicationStatus expected)
    {
        if (Status != expected)
            throw new DomainException(
                $"Application must be {expected}");
    }
}
