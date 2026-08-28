USE DVLD2p0;
GO

/* =========================================================
   Audit Logs
   Append-only audit trail
   ========================================================= */

CREATE TABLE dbo.AuditLogs
(
    LogID             BIGINT IDENTITY(1,1) NOT NULL,

    UserID            UNIQUEIDENTIFIER NULL,

    ActionType        TINYINT NOT NULL,

    EntityName        NVARCHAR(128) NOT NULL,

    EntityID          NVARCHAR(100) NULL,

    OldValues         NVARCHAR(MAX) NULL,

    NewValues         NVARCHAR(MAX) NULL,

    [Timestamp]       DATETIME2(3) NOT NULL
    CONSTRAINT DF_AuditLogs_Timestamp
    DEFAULT SYSUTCDATETIME(),

    IPAddress         VARCHAR(45) NULL,

    MachineName       NVARCHAR(255) NULL,


    /* =====================================================
       Primary Key
       ===================================================== */

    CONSTRAINT PK_AuditLogs
        PRIMARY KEY CLUSTERED (LogID),


    /* =====================================================
       Foreign Keys
       ===================================================== */

    CONSTRAINT FK_AuditLogs_User
        FOREIGN KEY (UserID)
            REFERENCES dbo.Users(UserID)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,


    /* =====================================================
       Constraints
       ===================================================== */

    CONSTRAINT CK_AuditLogs_ActionType
        CHECK (ActionType IN (1, 2, 3))
    );
GO