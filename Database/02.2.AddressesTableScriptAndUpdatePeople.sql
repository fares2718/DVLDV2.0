USE DVLD2p0;
GO

/* =========================================================
   Create Addresses
   ========================================================= */

CREATE TABLE dbo.Addresses
(
    AddressID           UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT DF_Addresses_AddressID
        DEFAULT NEWID(),

    PersonID            UNIQUEIDENTIFIER NOT NULL,

    AddressType         TINYINT NOT NULL
        CONSTRAINT DF_Addresses_AddressType
        DEFAULT 1,

    CountryCode         CHAR(2) NOT NULL,

    Governorate         NVARCHAR(100) NULL,

    City                NVARCHAR(100) NOT NULL,

    Street              NVARCHAR(200) NULL,

    BuildingNumber      NVARCHAR(50) NULL,

    ApartmentNumber     NVARCHAR(50) NULL,

    PostalCode          NVARCHAR(20) NULL,

    AdditionalDetails   NVARCHAR(500) NULL,

    IsPrimary           BIT NOT NULL
        CONSTRAINT DF_Addresses_IsPrimary
        DEFAULT 0,

    IsActive            BIT NOT NULL
        CONSTRAINT DF_Addresses_IsActive
        DEFAULT 1,

    CreatedAt           DATETIME2(3) NOT NULL
                        CONSTRAINT DF_Addresses_CreatedAt
                        DEFAULT SYSUTCDATETIME(),

    UpdatedAt           DATETIME2(3) NULL,

    CONSTRAINT PK_Addresses
        PRIMARY KEY CLUSTERED (AddressID),

    CONSTRAINT FK_Addresses_Person
        FOREIGN KEY (PersonID)
            REFERENCES dbo.People(PersonID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_Addresses_Country
        FOREIGN KEY (CountryCode)
            REFERENCES dbo.Countries(CountryCode)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT CK_Addresses_AddressType
        CHECK (AddressType IN (1, 2, 3))
);
GO


/* =========================================================
   Indexes
   ========================================================= */

CREATE INDEX IX_Addresses_PersonID
    ON dbo.Addresses(PersonID);
GO

CREATE UNIQUE INDEX UX_Addresses_Primary
    ON dbo.Addresses(PersonID)
    WHERE IsPrimary = 1;
GO


/* =========================================================
   Prevent deletion
   ========================================================= */

CREATE TRIGGER TR_Addresses_PreventDelete
    ON dbo.Addresses
    INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    THROW 50004,
        'Addresses cannot be deleted. Deactivate the address by setting IsActive = 0 instead.',
        1;
END;
GO


/* =========================================================
   Remove old Address column from People
   ========================================================= */

ALTER TABLE dbo.People
DROP COLUMN Address;
GO