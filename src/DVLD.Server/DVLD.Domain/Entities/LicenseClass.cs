using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public class LicenseClass
{
    public int LicenseClassId { get; private set; }

    public string ClassName { get; private set; } = null!;

    public string? ClassDescription { get; private set; }

    public byte MinimumAllowedAge { get; private set; }

    public byte MaximumAllowedAge { get; private set; }

    public byte DefaultValidityYears { get; private set; }

    public decimal ClassFees { get; private set; }

    public byte PrivilegeLevel { get; private set; }

    public int? ParentClassId { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public byte PrivilegeBit { get; private set; }

    // For EF Core
    private LicenseClass()
    {
    }

    private LicenseClass(
        string className,
        string? classDescription,
        byte minimumAllowedAge,
        byte maximumAllowedAge,
        byte defaultValidityYears,
        decimal classFees,
        byte privilegeLevel,
        int? parentClassId,
        byte privilegeBit)
    {
        ClassName = className;
        ClassDescription = classDescription;
        MinimumAllowedAge = minimumAllowedAge;
        MaximumAllowedAge = maximumAllowedAge;
        DefaultValidityYears = defaultValidityYears;
        ClassFees = classFees;
        PrivilegeLevel = privilegeLevel;
        ParentClassId = parentClassId;
        PrivilegeBit = privilegeBit;

        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static LicenseClass Create(
        string className,
        string? classDescription,
        byte minimumAllowedAge,
        byte maximumAllowedAge,
        byte defaultValidityYears,
        decimal classFees,
        byte privilegeLevel,
        int? parentClassId,
        byte privilegeBit)
    {
        if (string.IsNullOrWhiteSpace(className))
            throw new DomainException(
                "License Class Name cannot be empty");

        if (minimumAllowedAge == 0)
            throw new DomainException(
                "Minimum Allowed Age must be greater than zero");

        if (maximumAllowedAge < minimumAllowedAge)
            throw new DomainException(
                "Maximum Allowed Age cannot be less than Minimum Allowed Age");

        if (defaultValidityYears == 0)
            throw new DomainException(
                "Default Validity Years must be greater than zero");

        if (classFees < 0)
            throw new DomainException(
                "Class Fees cannot be negative");

        if (privilegeLevel == 0)
            throw new DomainException(
                "Privilege Level must be greater than zero");

        if (privilegeBit == 0)
            throw new DomainException(
                "Privilege Bit must be greater than zero");

        if (parentClassId is <= 0)
            throw new DomainException(
                "Parent Class ID must be positive");

        return new LicenseClass(
            className,
            classDescription,
            minimumAllowedAge,
            maximumAllowedAge,
            defaultValidityYears,
            classFees,
            privilegeLevel,
            parentClassId,
            privilegeBit);
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException(
                "License Class is already inactive");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException(
                "License Class is already active");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateAgeRange(
        byte minimumAllowedAge,
        byte maximumAllowedAge)
    {
        if (minimumAllowedAge == 0)
            throw new DomainException(
                "Minimum Allowed Age must be greater than zero");

        if (maximumAllowedAge < minimumAllowedAge)
            throw new DomainException(
                "Maximum Allowed Age cannot be less than Minimum Allowed Age");

        MinimumAllowedAge = minimumAllowedAge;
        MaximumAllowedAge = maximumAllowedAge;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateFees(decimal classFees)
    {
        if (classFees < 0)
            throw new DomainException(
                "Class Fees cannot be negative");

        ClassFees = classFees;
        UpdatedAt = DateTime.UtcNow;
    }

}