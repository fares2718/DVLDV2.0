USE DVLD2p0;
GO

/* =========================================================
   Test Types
   ========================================================= */

CREATE TABLE dbo.TestTypes
(
    TestTypeID          INT IDENTITY(1,1) NOT NULL,

    Title               NVARCHAR(150) NOT NULL,

    Description         NVARCHAR(500) NULL,

    Fees                DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_TestTypes_Fees
            DEFAULT 0,

    OrderInSequence     TINYINT NOT NULL,

    IsActive             BIT NOT NULL
        CONSTRAINT DF_TestTypes_IsActive
            DEFAULT 1,

    CreatedAt           DATETIME2(3) NOT NULL
        CONSTRAINT DF_TestTypes_CreatedAt
            DEFAULT SYSUTCDATETIME(),

    UpdatedAt           DATETIME2(3) NULL,

    CONSTRAINT PK_TestTypes
        PRIMARY KEY CLUSTERED (TestTypeID),

    CONSTRAINT UQ_TestTypes_Title
        UNIQUE (Title),

    CONSTRAINT UQ_TestTypes_OrderInSequence
        UNIQUE (OrderInSequence),

    CONSTRAINT CK_TestTypes_Fees
        CHECK (Fees >= 0),

    CONSTRAINT CK_TestTypes_OrderInSequence
        CHECK (OrderInSequence > 0)
);
GO

INSERT INTO dbo.TestTypes
(
    Title,
    Description,
    Fees,
    OrderInSequence
)
VALUES
    (
        N'Vision Test',
        N'Eye and vision examination required before proceeding with the driving tests.',
        10.00,
        1
    ),
    (
        N'Written (Theory) Test',
        N'Theoretical examination covering traffic laws, signs, and driving regulations.',
        15.00,
        2
    ),
    (
        N'Practical (Street) Test',
        N'Practical driving examination conducted on the road.',
        30.00,
        3
    );
GO

/* =========================================================
   Test Appointments
   ========================================================= */

CREATE TABLE dbo.TestAppointments
(
    TestAppointmentID    INT IDENTITY(1,1) NOT NULL,

    ApplicationID        INT NOT NULL,

    TestTypeID           INT NOT NULL,

    AppointmentDate      DATETIME2(3) NOT NULL,

    PaidFees             DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_TestAppointments_PaidFees
            DEFAULT 0,

    IsLocked             BIT NOT NULL
        CONSTRAINT DF_TestAppointments_IsLocked
            DEFAULT 0,

    CreatedByUserID      UNIQUEIDENTIFIER NOT NULL,

    CreatedAt            DATETIME2(3) NOT NULL
        CONSTRAINT DF_TestAppointments_CreatedAt
            DEFAULT SYSUTCDATETIME(),

    UpdatedAt            DATETIME2(3) NULL,

    CONSTRAINT PK_TestAppointments
        PRIMARY KEY CLUSTERED (TestAppointmentID),

    CONSTRAINT FK_TestAppointments_Application
        FOREIGN KEY (ApplicationID)
            REFERENCES dbo.Applications(ApplicationID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_TestAppointments_TestType
        FOREIGN KEY (TestTypeID)
            REFERENCES dbo.TestTypes(TestTypeID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_TestAppointments_CreatedByUser
        FOREIGN KEY (CreatedByUserID)
            REFERENCES dbo.Users(UserID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT CK_TestAppointments_PaidFees
        CHECK (PaidFees >= 0)
);
GO

CREATE INDEX IX_TestAppointments_ApplicationID
    ON dbo.TestAppointments(ApplicationID);
GO

CREATE INDEX IX_TestAppointments_TestTypeID
    ON dbo.TestAppointments(TestTypeID);
GO

CREATE INDEX IX_TestAppointments_AppointmentDate
    ON dbo.TestAppointments(AppointmentDate);
GO

CREATE INDEX IX_TestAppointments_CreatedByUserID
    ON dbo.TestAppointments(CreatedByUserID);
GO

/* =========================================================
   Tests - Results
   ========================================================= */

CREATE TABLE dbo.Tests
(
    TestID               INT IDENTITY(1,1) NOT NULL,

    TestAppointmentID    INT NOT NULL,

    TestResult           BIT NOT NULL,

    Notes                NVARCHAR(1000) NULL,

    CreatedByUserID      UNIQUEIDENTIFIER NOT NULL,

    CreatedAt            DATETIME2(3) NOT NULL
        CONSTRAINT DF_Tests_CreatedAt
            DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Tests
        PRIMARY KEY CLUSTERED (TestID),

    CONSTRAINT UQ_Tests_TestAppointmentID
        UNIQUE (TestAppointmentID),

    CONSTRAINT FK_Tests_TestAppointment
        FOREIGN KEY (TestAppointmentID)
            REFERENCES dbo.TestAppointments(TestAppointmentID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,

    CONSTRAINT FK_Tests_CreatedByUser
        FOREIGN KEY (CreatedByUserID)
            REFERENCES dbo.Users(UserID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION
);
GO