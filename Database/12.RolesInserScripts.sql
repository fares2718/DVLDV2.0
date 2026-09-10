USE DVLD2p0;
GO

/* =========================================================
   System Administrator
   Full access to all Phase 1 permissions.

   Permissions:
   Bits 0 through 24
   ========================================================= */

INSERT INTO dbo.Roles
(
    Name,
    Description,
    Permissions,
    IsSystemRole,
    IsActive,
    CreatedAt,
    UpdatedAt
)
VALUES
(
    N'System Administrator',
    N'Full access to system users, roles, configuration, master data and all operational features.',
    33554431,
    1,
    1,
    SYSUTCDATETIME(),
    NULL
);
GO


/* =========================================================
   License Officer

   Can:
   - Manage People
   - Manage Applications
   - View Licenses
   - Issue Licenses
   - Renew Licenses
   - Replace Licenses
   ========================================================= */

INSERT INTO dbo.Roles
(
    Name,
    Description,
    Permissions,
    IsSystemRole,
    IsActive,
    CreatedAt,
    UpdatedAt
)
VALUES
(
    N'License Officer',
    N'Handles people, driving license applications, license issuance, renewal and replacement.',

    /*
       ViewPeople            = 1
       CreatePeople          = 2
       EditPeople            = 4

       ViewApplications      = 8
       CreateApplications    = 16
       EditApplications      = 32
       CancelApplications    = 64

       ViewLicenses          = 128
       IssueLicenses         = 256
       RenewLicenses         = 512
       ReplaceLicenses       = 1024

       Total = 2047
    */

    2047,
    1,
    1,
    SYSUTCDATETIME(),
    NULL
);
GO


/* =========================================================
   Test Scheduler

   Can:
   - View People
   - View Applications
   - View Tests
   - Schedule Tests
   - Record Test Results
   ========================================================= */

INSERT INTO dbo.Roles
(
    Name,
    Description,
    Permissions,
    IsSystemRole,
    IsActive,
    CreatedAt,
    UpdatedAt
)
VALUES
(
    N'Test Scheduler',
    N'Schedules driving tests and records test results.',

    /*
       ViewPeople          = 1
       ViewApplications    = 8

       ViewTests           = 2048
       ScheduleTests       = 4096
       RecordTestResults   = 8192

       Total = 14345
    */

    14345,
    1,
    1,
    SYSUTCDATETIME(),
    NULL
);
GO


/* =========================================================
   Detention Officer

   Can:
   - View People
   - View Licenses
   - View Detentions
   - Detain Licenses
   - Release Licenses
   - Manage Fines
   ========================================================= */

INSERT INTO dbo.Roles
(
    Name,
    Description,
    Permissions,
    IsSystemRole,
    IsActive,
    CreatedAt,
    UpdatedAt
)
VALUES
(
    N'Detention Officer',
    N'Handles license detention, release and detention fines.',

    /*
       ViewPeople        = 1
       ViewLicenses      = 128

       ViewDetentions    = 16384
       DetainLicenses    = 32768
       ReleaseLicenses   = 65536
       ManageFines       = 131072

       Total = 245889
    */

    245889,
    1,
    1,
    SYSUTCDATETIME(),
    NULL
);
GO


/* =========================================================
   Read-Only / Inquiry

   Can only view:
   - People
   - Applications
   - Licenses
   - Tests
   - Detentions
   ========================================================= */

INSERT INTO dbo.Roles
(
    Name,
    Description,
    Permissions,
    IsSystemRole,
    IsActive,
    CreatedAt,
    UpdatedAt
)
VALUES
(
    N'Read-Only Inquiry',
    N'Read-only access for searching and viewing system information.',

    /*
       ViewPeople        = 1
       ViewApplications  = 8
       ViewLicenses      = 128
       ViewTests         = 2048
       ViewDetentions    = 16384

       Total = 18569
    */

    18569,
    1,
    1,
    SYSUTCDATETIME(),
    NULL
);
GO


/* =========================================================
   Supervisor / Manager

   Can:
   - View operational data
   - Cancel applications
   - View audit logs
   - View master data
   ========================================================= */

INSERT INTO dbo.Roles
(
    Name,
    Description,
    Permissions,
    IsSystemRole,
    IsActive,
    CreatedAt,
    UpdatedAt
)
VALUES
(
    N'Supervisor',
    N'Supervises operations, reviews sensitive activities and monitors audit information.',

    /*
       ViewPeople          = 1
       ViewApplications    = 8
       CancelApplications  = 64

       ViewLicenses        = 128

       ViewTests           = 2048

       ViewDetentions      = 16384

       ViewUsers           = 262144
       ViewRoles           = 1048576

       ViewMasterData      = 4194304

       ViewAuditLogs       = 16777216

       Total = 23277641
    */

    23277641,
    1,
    1,
    SYSUTCDATETIME(),
    NULL
);
GO


/* =========================================================
   Driver Self-Service
   Phase 2

   Can only:
   - View own data
   - Submit applications
   - Upload documents
   - Track own applications
   ========================================================= */

INSERT INTO dbo.Roles
(
    Name,
    Description,
    Permissions,
    IsSystemRole,
    IsActive,
    CreatedAt,
    UpdatedAt
)
VALUES
(
    N'Driver',
    N'Driver self-service role for viewing personal information and managing own applications.',

    /*
       ViewOwnData             = 33554432
       SubmitApplications      = 67108864
       UploadDocuments         = 134217728
       TrackOwnApplications    = 268435456

       Total = 503316480
    */

    503316480,
    1,
    1,
    SYSUTCDATETIME(),
    NULL
);
GO
