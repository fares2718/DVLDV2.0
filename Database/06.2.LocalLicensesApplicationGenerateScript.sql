USE DVLD2p0;
GO

/* =========================================================
   Local Driving License Applications
   Specialization of Applications for local licenses
   ========================================================= */

CREATE TABLE dbo.LocalDrivingLicenseApplications
(
    LocalDrivingLicenseApplicationID INT IDENTITY(1,1) NOT NULL,

    ApplicationID                   INT NOT NULL,

    LicenseClassID                  INT NOT NULL,

    CONSTRAINT PK_LocalDrivingLicenseApplications
        PRIMARY KEY CLUSTERED (LocalDrivingLicenseApplicationID),

    CONSTRAINT UQ_LocalDrivingLicenseApplications_ApplicationID
        UNIQUE (ApplicationID),

    CONSTRAINT FK_LocalDrivingLicenseApplications_Application
        FOREIGN KEY (ApplicationID)
            REFERENCES dbo.Applications(ApplicationID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_LocalDrivingLicenseApplications_LicenseClass
        FOREIGN KEY (LicenseClassID)
            REFERENCES dbo.LicenseClasses(LicenseClassID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION
);
GO