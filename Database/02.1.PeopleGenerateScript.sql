USE DVLD2p0;
GO

CREATE TABLE dbo.People
(
    PersonID                    UNIQUEIDENTIFIER DEFAULT NEWID(),
    NationalID                  NVARCHAR(50)  UNIQUE NOT NULL,

    FirstName                   NVARCHAR(100)   NOT NULL,
    SecondName                  NVARCHAR(100)   NOT NULL,
    ThirdName                   NVARCHAR(100)   NULL,
    LastName                    NVARCHAR(100)   NOT NULL,

    MotherName                  NVARCHAR(200)   NOT NULL,
    DateOfBirth                 DATE            NOT NULL,
    Address                     NVARCHAR(500)   NULL,
    Phone                       NVARCHAR(30)    NOT NULL,
    AltPhone                       NVARCHAR(30)    NULL,
    Gender                      BIT         NOT NULL,
    Email                       NVARCHAR(254)   NOT NULL,
    NationalityCountryCode      CHAR(2)         NOT NULL,
    ImagePath                       NVARCHAR(500)   NULL,

    CreatedAt                 DATETIME2(0)    NOT NULL
        CONSTRAINT DF_People_CreatedAt
            DEFAULT SYSUTCDATETIME(),

    IsActive                    BIT             NOT NULL
        CONSTRAINT DF_People_IsActive
            DEFAULT 1,

    CONSTRAINT PK_People
        PRIMARY KEY CLUSTERED (PersonID),

    CONSTRAINT FK_People_Countries
        FOREIGN KEY (NationalityCountryCode)
            REFERENCES dbo.Countries (CountryCode)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,
);
GO


/* =========================================================
   Indexes
   ========================================================= */

CREATE INDEX IX_People_Name
    ON dbo.People
        (
         LastName,
         FirstName,
         SecondName,
         ThirdName
            );
GO

CREATE INDEX IX_People_Phone
    ON dbo.People (Phone);
GO

CREATE INDEX IX_People_Email
    ON dbo.People (Email);
GO


/* =========================================================
   Prevent deletion of people
   ========================================================= */

CREATE TRIGGER TR_People_PreventDelete
    ON dbo.People
    INSTEAD OF DELETE
    AS
BEGIN
    SET NOCOUNT ON;

    THROW 50001,
        'People cannot be deleted. Deactivate the person by setting IsActive = 0 instead.',
        1;
END;
GO