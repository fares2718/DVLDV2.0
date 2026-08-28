USE DVLD2p0;
GO

/* =========================================================
   License Classes
   ========================================================= */

CREATE TABLE dbo.LicenseClasses
(
    LicenseClassID          INT IDENTITY(1,1) NOT NULL,

    ClassName               NVARCHAR(100) NOT NULL,

    ClassDescription        NVARCHAR(500) NULL,

    MinimumAllowedAge       TINYINT NOT NULL,
    MaximumAllowedAge       TINYINT NOT NULL,

    DefaultValidityYears    TINYINT NOT NULL,

    ClassFees               DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_LicenseClasses_ClassFees
            DEFAULT 0,

    PrivilegeLevel          TINYINT NOT NULL,

    ParentClassID           INT NULL,

    IsActive                BIT NOT NULL
        CONSTRAINT DF_LicenseClasses_IsActive
            DEFAULT 1,

    CreatedAt               DATETIME2(3) NOT NULL
        CONSTRAINT DF_LicenseClasses_CreatedAt
            DEFAULT SYSUTCDATETIME(),

    UpdatedAt               DATETIME2(3) NULL,

    CONSTRAINT PK_LicenseClasses
        PRIMARY KEY CLUSTERED (LicenseClassID),

    CONSTRAINT UQ_LicenseClasses_ClassName
        UNIQUE (ClassName),

    CONSTRAINT FK_LicenseClasses_Parent
        FOREIGN KEY (ParentClassID)
            REFERENCES dbo.LicenseClasses(LicenseClassID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT CK_LicenseClasses_MinimumAge
        CHECK (MinimumAllowedAge > 0),

    CONSTRAINT CK_LicenseClasses_MaximumAge
        CHECK (MaximumAllowedAge > 0),

    CONSTRAINT CK_LicenseClasses_ValidityYears
        CHECK (DefaultValidityYears > 0),

    CONSTRAINT CK_LicenseClasses_ClassFees
        CHECK (ClassFees >= 0),

    CONSTRAINT CK_LicenseClasses_PrivilegeLevel
        CHECK (PrivilegeLevel > 0),

    CONSTRAINT CK_LicenseClasses_NotSelfParent
        CHECK
            (
            ParentClassID IS NULL
                OR ParentClassID <> LicenseClassID
            )
);
GO


/* =========================================================
   Indexes
   ========================================================= */

CREATE INDEX IX_LicenseClasses_ParentClassID
    ON dbo.LicenseClasses(ParentClassID);
GO

/* =========================================================
   Local Driving Licenses
   ========================================================= */

CREATE TABLE dbo.Licenses
(
    LicenseID           UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT DF_Licenses_LicenseID
            DEFAULT NEWID(),

    DriverID            UNIQUEIDENTIFIER NOT NULL,

    LicenseClassID      INT NOT NULL,

    IssueDate           DATE NOT NULL,

    ExpirationDate      DATE NOT NULL,

    IsActive            BIT NOT NULL
        CONSTRAINT DF_Licenses_IsActive
            DEFAULT 1,

    IssueReason         TINYINT NOT NULL,

    Notes               NVARCHAR(1000) NULL,

    ApplicationID       INT NOT NULL,

    CreatedAt           DATETIME2(3) NOT NULL
        CONSTRAINT DF_Licenses_CreatedAt
            DEFAULT SYSUTCDATETIME(),

    UpdatedAt           DATETIME2(3) NULL,

    CONSTRAINT PK_Licenses
        PRIMARY KEY CLUSTERED (LicenseID),

    CONSTRAINT FK_Licenses_Driver
        FOREIGN KEY (DriverID)
            REFERENCES dbo.Drivers(DriverID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_Licenses_LicenseClass
        FOREIGN KEY (LicenseClassID)
            REFERENCES dbo.LicenseClasses(LicenseClassID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_Licenses_Application
        FOREIGN KEY (ApplicationID)
            REFERENCES dbo.Applications(ApplicationID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT CK_Licenses_Dates
        CHECK (ExpirationDate > IssueDate),

    CONSTRAINT CK_Licenses_IssueReason
        CHECK (IssueReason IN (1, 2, 3, 4, 5, 6))
);
GO


/* =========================================================
   Indexes
   ========================================================= */

CREATE INDEX IX_Licenses_DriverID
    ON dbo.Licenses(DriverID);
GO

CREATE INDEX IX_Licenses_LicenseClassID
    ON dbo.Licenses(LicenseClassID);
GO

CREATE INDEX IX_Licenses_ApplicationID
    ON dbo.Licenses(ApplicationID);
GO

CREATE INDEX IX_Licenses_ExpirationDate
    ON dbo.Licenses(ExpirationDate);
GO

/* =========================================================
   International Licenses
   ========================================================= */

CREATE TABLE dbo.InternationalLicenses
(
    InternationalLicenseID      UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT DF_InternationalLicenses_ID
            DEFAULT NEWID(),

    DriverID                    UNIQUEIDENTIFIER NOT NULL,

    IssuedUsingLocalLicenseID   UNIQUEIDENTIFIER NOT NULL,

    IssueDate                   DATE NOT NULL,

    ExpirationDate              DATE NOT NULL,

    IsActive                    BIT NOT NULL
        CONSTRAINT DF_InternationalLicenses_IsActive
            DEFAULT 1,

    ApplicationID               INT NOT NULL,

    CreatedAt                   DATETIME2(3) NOT NULL
        CONSTRAINT DF_InternationalLicenses_CreatedAt
            DEFAULT SYSUTCDATETIME(),

    UpdatedAt                   DATETIME2(3) NULL,

    CONSTRAINT PK_InternationalLicenses
        PRIMARY KEY CLUSTERED (InternationalLicenseID),

    CONSTRAINT FK_InternationalLicenses_Driver
        FOREIGN KEY (DriverID)
            REFERENCES dbo.Drivers(DriverID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_InternationalLicenses_LocalLicense
        FOREIGN KEY (IssuedUsingLocalLicenseID)
            REFERENCES dbo.Licenses(LicenseID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_InternationalLicenses_Application
        FOREIGN KEY (ApplicationID)
            REFERENCES dbo.Applications(ApplicationID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT CK_InternationalLicenses_Dates
        CHECK (ExpirationDate > IssueDate)
);
GO


/* =========================================================
   Indexes
   ========================================================= */

CREATE INDEX IX_InternationalLicenses_DriverID
    ON dbo.InternationalLicenses(DriverID);
GO

CREATE INDEX IX_InternationalLicenses_LocalLicenseID
    ON dbo.InternationalLicenses(IssuedUsingLocalLicenseID);
GO

CREATE INDEX IX_InternationalLicenses_ApplicationID
    ON dbo.InternationalLicenses(ApplicationID);
GO

ALTER TABLE dbo.Applications
    ADD CONSTRAINT FK_Applications_RelatedLicense
        FOREIGN KEY (RelatedLicenseID)
            REFERENCES dbo.Licenses(LicenseID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION;
GO
