USE DVLD2p0;
GO

/* =========================================================
   Drivers
   ========================================================= */

CREATE TABLE dbo.Drivers
(
    DriverID        UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT DF_Drivers_DriverID
            DEFAULT NEWID(),

    PersonID        UNIQUEIDENTIFIER NOT NULL,

    IsActive        BIT NOT NULL
        CONSTRAINT DF_Drivers_IsActive
            DEFAULT 1,

    CreatedAt       DATETIME2(3) NOT NULL
        CONSTRAINT DF_Drivers_CreatedAt
            DEFAULT SYSUTCDATETIME(),

    UpdatedAt       DATETIME2(3) NULL,

    CONSTRAINT PK_Drivers
        PRIMARY KEY (DriverID),

    CONSTRAINT UQ_Drivers_PersonID
        UNIQUE (PersonID),

    CONSTRAINT FK_Drivers_Person
        FOREIGN KEY (PersonID)
            REFERENCES dbo.People(PersonID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION
);
GO


/* =========================================================
   Prevent deletion of drivers
   ========================================================= */

CREATE TRIGGER TR_Drivers_PreventDelete
    ON dbo.Drivers
    INSTEAD OF DELETE
    AS
BEGIN
    SET NOCOUNT ON;

    THROW 50003,
        'Drivers cannot be deleted. Deactivate the driver by setting IsActive = 0 instead.',
        1;
END;
GO