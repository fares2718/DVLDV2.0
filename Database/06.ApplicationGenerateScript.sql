USE DVLD2p0;
GO

/* =========================================================
   Application Types
   ========================================================= */

CREATE TABLE dbo.ApplicationTypes
(
    ApplicationTypeID   INT IDENTITY(1,1) NOT NULL,

    Title               NVARCHAR(150) NOT NULL,

    BaseFees            DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_ApplicationTypes_BaseFees
            DEFAULT 0,

    Description         NVARCHAR(500) NULL,

    IsActive            BIT NOT NULL
        CONSTRAINT DF_ApplicationTypes_IsActive
            DEFAULT 1,

    CreatedAt           DATETIME2(3) NOT NULL
        CONSTRAINT DF_ApplicationTypes_CreatedAt
            DEFAULT SYSUTCDATETIME(),

    UpdatedAt           DATETIME2(3) NULL,

    CONSTRAINT PK_ApplicationTypes
        PRIMARY KEY CLUSTERED (ApplicationTypeID),

    CONSTRAINT UQ_ApplicationTypes_Title
        UNIQUE (Title),

    CONSTRAINT CK_ApplicationTypes_BaseFees
        CHECK (BaseFees >= 0)
);
GO


/* =========================================================
   Applications
   ========================================================= */

CREATE TABLE dbo.Applications
(
    ApplicationID          INT IDENTITY(1,1) NOT NULL,

    ApplicantPersonID      UNIQUEIDENTIFIER NOT NULL,

    ApplicationTypeID      INT NOT NULL,

    ApplicationDate        DATETIME2(3) NOT NULL
        CONSTRAINT DF_Applications_ApplicationDate
            DEFAULT SYSUTCDATETIME(),

    Status                 TINYINT NOT NULL
        CONSTRAINT DF_Applications_Status
            DEFAULT 1,

    PaidFees               DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_Applications_PaidFees
            DEFAULT 0,

    CreatedByUserID        UNIQUEIDENTIFIER NOT NULL,

    LastStatusDate         DATETIME2(3) NOT NULL
        CONSTRAINT DF_Applications_LastStatusDate
            DEFAULT SYSUTCDATETIME(),

    RelatedLicenseID       UNIQUEIDENTIFIER NULL,

    RelatedApplicationID   INT NULL,

    CreatedAt              DATETIME2(3) NOT NULL
        CONSTRAINT DF_Applications_CreatedAt
            DEFAULT SYSUTCDATETIME(),

    UpdatedAt              DATETIME2(3) NULL,


    /* =====================================================
       Primary Key
       ===================================================== */

    CONSTRAINT PK_Applications
        PRIMARY KEY CLUSTERED (ApplicationID),



    /* =====================================================
       Foreign Keys
       ===================================================== */

    CONSTRAINT FK_Applications_ApplicantPerson
        FOREIGN KEY (ApplicantPersonID)
            REFERENCES dbo.People(PersonID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_Applications_ApplicationType
        FOREIGN KEY (ApplicationTypeID)
            REFERENCES dbo.ApplicationTypes(ApplicationTypeID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_Applications_CreatedByUser
        FOREIGN KEY (CreatedByUserID)
            REFERENCES dbo.Users(UserID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_Applications_RelatedApplication
        FOREIGN KEY (RelatedApplicationID)
            REFERENCES dbo.Applications(ApplicationID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,


    /* =====================================================
       Constraints
       ===================================================== */

    CONSTRAINT CK_Applications_Status
        CHECK (Status IN (1, 2, 3, 4, 5)),

    CONSTRAINT CK_Applications_PaidFees
        CHECK (PaidFees >= 0),

    CONSTRAINT CK_Applications_NotSelfRelated
        CHECK
            (
            RelatedApplicationID IS NULL
                OR RelatedApplicationID <> ApplicationID
            )
);
GO


/* =========================================================
   Indexes
   ========================================================= */

CREATE INDEX IX_Applications_ApplicantPersonID
    ON dbo.Applications(ApplicantPersonID);
GO

CREATE INDEX IX_Applications_ApplicationTypeID
    ON dbo.Applications(ApplicationTypeID);
GO

CREATE INDEX IX_Applications_CreatedByUserID
    ON dbo.Applications(CreatedByUserID);
GO

CREATE INDEX IX_Applications_RelatedLicenseID
    ON dbo.Applications(RelatedLicenseID);
GO

CREATE INDEX IX_Applications_RelatedApplicationID
    ON dbo.Applications(RelatedApplicationID);
GO

CREATE INDEX IX_Applications_Status
    ON dbo.Applications(Status);
GO

INSERT INTO dbo.ApplicationTypes
(
    Title,
    BaseFees,
    Description,
    IsActive
)
VALUES
    (
        N'New Local Driving License',
        50.00,
        N'Application for issuing a new local driving license.',
        1
    ),
    (
        N'Retake Test',
        25.00,
        N'Application for retaking a failed driving test.',
        1
    ),
    (
        N'Renew Driving License',
        40.00,
        N'Application for renewing an existing driving license.',
        1
    ),
    (
        N'Replace Lost License',
        75.00,
        N'Application for replacing a lost driving license.',
        1
    ),
    (
        N'Replace Damaged License',
        60.00,
        N'Application for replacing a damaged driving license.',
        1
    ),
    (
        N'Release Detained License',
        100.00,
        N'Application for releasing a detained driving license.',
        1
    ),
    (
        N'International License',
        80.00,
        N'Application for issuing an international driving license.',
        1
    );
GO