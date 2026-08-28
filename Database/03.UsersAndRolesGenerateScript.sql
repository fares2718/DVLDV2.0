USE DVLD2p0;
GO

/* =========================================================
   Roles
   ========================================================= */

CREATE TABLE dbo.Roles
(
    RoleID          INT IDENTITY(1,1) NOT NULL,

    Name            NVARCHAR(100) NOT NULL,
    Description     NVARCHAR(500) NULL,

    Permissions     BIGINT NOT NULL
        CONSTRAINT DF_Roles_Permissions DEFAULT 0,

    IsSystemRole    BIT NOT NULL
        CONSTRAINT DF_Roles_IsSystemRole DEFAULT 0,

    IsActive        BIT NOT NULL
        CONSTRAINT DF_Roles_IsActive DEFAULT 1,

    CreatedAt       DATETIME2(3) NOT NULL
        CONSTRAINT DF_Roles_CreatedAt
            DEFAULT SYSUTCDATETIME(),

    UpdatedAt       DATETIME2(3) NULL,

    CONSTRAINT PK_Roles
        PRIMARY KEY (RoleID),

    CONSTRAINT UQ_Roles_Name
        UNIQUE (Name)
);
GO


/* =========================================================
   Users
   ========================================================= */

CREATE TABLE dbo.Users
(
    UserID              UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT DF_Users_UserID DEFAULT NEWID(),

    PersonID            UNIQUEIDENTIFIER NOT NULL,

    Username            NVARCHAR(100) NOT NULL,

    PasswordHash        NVARCHAR(500) NOT NULL,

    IsActive            BIT NOT NULL
        CONSTRAINT DF_Users_IsActive DEFAULT 1,

    IsLocked            BIT NOT NULL
        CONSTRAINT DF_Users_IsLocked DEFAULT 0,

    FailedLoginAttempts INT NOT NULL
        CONSTRAINT DF_Users_FailedLoginAttempts DEFAULT 0,

    LockedUntil         DATETIME2(3) NULL,

    LastLoginAt         DATETIME2(3) NULL,

    PasswordChangedAt   DATETIME2(3) NULL,

    CreatedAt           DATETIME2(3) NOT NULL
        CONSTRAINT DF_Users_CreatedAt
            DEFAULT SYSUTCDATETIME(),

    UpdatedAt           DATETIME2(3) NULL,

    CONSTRAINT PK_Users
        PRIMARY KEY (UserID),

    CONSTRAINT UQ_Users_PersonID
        UNIQUE (PersonID),

    CONSTRAINT UQ_Users_Username
        UNIQUE (Username),

    CONSTRAINT CK_Users_FailedLoginAttempts
        CHECK (FailedLoginAttempts >= 0),

    CONSTRAINT FK_Users_Person
        FOREIGN KEY (PersonID)
            REFERENCES dbo.People(PersonID)
);
GO


/* =========================================================
   User Roles
   ========================================================= */

CREATE TABLE dbo.UserRoles
(
    UserID      UNIQUEIDENTIFIER NOT NULL,

    RoleID      INT NOT NULL,

    AssignedAt  DATETIME2(3) NOT NULL
        CONSTRAINT DF_UserRoles_AssignedAt
            DEFAULT SYSUTCDATETIME(),

    AssignedBy  UNIQUEIDENTIFIER NULL,

    CONSTRAINT PK_UserRoles
        PRIMARY KEY (UserID, RoleID),

    CONSTRAINT FK_UserRoles_User
        FOREIGN KEY (UserID)
            REFERENCES dbo.Users(UserID)
            ON DELETE CASCADE,

    CONSTRAINT FK_UserRoles_Role
        FOREIGN KEY (RoleID)
            REFERENCES dbo.Roles(RoleID),

    CONSTRAINT FK_UserRoles_AssignedBy
        FOREIGN KEY (AssignedBy)
            REFERENCES dbo.Users(UserID)
);
GO


/* =========================================================
   Indexes
   ========================================================= */

CREATE INDEX IX_UserRoles_RoleID
    ON dbo.UserRoles(RoleID);
GO


/* =========================================================
   Prevent deletion of users
   ========================================================= */

CREATE TRIGGER TR_Users_PreventDelete
    ON dbo.Users
    INSTEAD OF DELETE
    AS
BEGIN
    SET NOCOUNT ON;

    THROW 50002,
        'Users cannot be deleted. Deactivate the user by setting IsActive = 0 instead.',
        1;
END;
GO