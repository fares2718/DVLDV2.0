USE DVLD2p0;
GO

/* =========================================================
   Detentions
   ========================================================= */

CREATE TABLE dbo.Detentions
(
    DetentionID          INT IDENTITY(1,1) NOT NULL,

    LicenseID            UNIQUEIDENTIFIER NOT NULL,

    DetainDate           DATETIME2(3) NOT NULL
                         CONSTRAINT DF_Detentions_DetainDate
                         DEFAULT SYSUTCDATETIME(),

    FineFees             DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_Detentions_FineFees
        DEFAULT 0,

    CreatedByUserID      UNIQUEIDENTIFIER NOT NULL,

    IsReleased            BIT NOT NULL
        CONSTRAINT DF_Detentions_IsReleased
        DEFAULT 0,

    ReleaseDate          DATETIME2(3) NULL,

    ReleasedByUserID     UNIQUEIDENTIFIER NULL,

    ReleaseApplicationID INT NULL,

    CreatedAt            DATETIME2(3) NOT NULL
                         CONSTRAINT DF_Detentions_CreatedAt
                         DEFAULT SYSUTCDATETIME(),

    UpdatedAt            DATETIME2(3) NULL,


    /* =====================================================
       Primary Key
       ===================================================== */

    CONSTRAINT PK_Detentions
        PRIMARY KEY CLUSTERED (DetentionID),


    /* =====================================================
       Foreign Keys
       ===================================================== */

    CONSTRAINT FK_Detentions_License
        FOREIGN KEY (LicenseID)
            REFERENCES dbo.Licenses(LicenseID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_Detentions_CreatedByUser
        FOREIGN KEY (CreatedByUserID)
            REFERENCES dbo.Users(UserID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_Detentions_ReleasedByUser
        FOREIGN KEY (ReleasedByUserID)
            REFERENCES dbo.Users(UserID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_Detentions_ReleaseApplication
        FOREIGN KEY (ReleaseApplicationID)
            REFERENCES dbo.Applications(ApplicationID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,


    /* =====================================================
       Constraints
       ===================================================== */

    CONSTRAINT CK_Detentions_FineFees
        CHECK (FineFees >= 0),

    CONSTRAINT CK_Detentions_ReleaseData
        CHECK
            (
            (IsReleased = 0
                AND ReleaseDate IS NULL
                AND ReleasedByUserID IS NULL
                AND ReleaseApplicationID IS NULL)
                OR
            (IsReleased = 1
                AND ReleaseDate IS NOT NULL
                AND ReleasedByUserID IS NOT NULL
                AND ReleaseApplicationID IS NOT NULL)
            ),

    CONSTRAINT CK_Detentions_ReleaseDate
        CHECK
            (
            ReleaseDate IS NULL
                OR ReleaseDate >= DetainDate
            )
);
GO


/* =========================================================
   Indexes
   ========================================================= */

CREATE INDEX IX_Detentions_LicenseID
    ON dbo.Detentions(LicenseID);
GO

CREATE INDEX IX_Detentions_CreatedByUserID
    ON dbo.Detentions(CreatedByUserID);
GO

CREATE INDEX IX_Detentions_ReleasedByUserID
    ON dbo.Detentions(ReleasedByUserID);
GO

CREATE INDEX IX_Detentions_ReleaseApplicationID
    ON dbo.Detentions(ReleaseApplicationID);
GO

CREATE INDEX IX_Detentions_IsReleased
    ON dbo.Detentions(IsReleased);
GO