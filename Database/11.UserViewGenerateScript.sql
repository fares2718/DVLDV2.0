USE DVLD2p0;
GO

/* =========================================================
   View: vw_Users
   Purpose:
   Lightweight user list for:
   - Pagination
   - Filtering
   - Sorting
   ========================================================= */

CREATE OR ALTER VIEW dbo.vw_Users
AS
SELECT
    U.UserID,
    U.PersonID,

    U.Username,

    CONCAT(
        P.FirstName, N' ',
        P.SecondName, N' ',
        COALESCE(P.ThirdName + N' ', N''),
        P.LastName
    ) AS FullName,

    P.NationalID,
    P.Phone,
    P.Email,

    U.IsActive,
    U.IsLocked,

    STRING_AGG(R.Name, N', ') AS Roles

FROM dbo.Users AS U

INNER JOIN dbo.People AS P
    ON P.PersonID = U.PersonID

LEFT JOIN dbo.UserRoles AS UR
    ON UR.UserID = U.UserID

LEFT JOIN dbo.Roles AS R
    ON R.RoleID = UR.RoleID

GROUP BY
    U.UserID,
    U.PersonID,
    U.Username,

    P.FirstName,
    P.SecondName,
    P.ThirdName,
    P.LastName,

    P.NationalID,
    P.Phone,
    P.Email,

    U.IsActive,
    U.IsLocked;
GO


USE DVLD2p0;
GO

/* =========================================================
   View: vw_UserDetails
   Purpose:
   Complete user information for:
   GET /api/users/{id}
   ========================================================= */

CREATE OR ALTER VIEW dbo.vw_UserDetails
AS
SELECT
    /* =====================================================
       User
       ===================================================== */

    U.UserID,
    U.PersonID,

    U.Username,

    U.IsActive AS IsUserActive,
    U.IsLocked,

    U.FailedLoginAttempts,
    U.LockedUntil,
    U.LastLoginAt,
    U.PasswordChangedAt,

    U.CreatedAt AS UserCreatedAt,
    U.UpdatedAt AS UserUpdatedAt,


    /* =====================================================
       Person
       ===================================================== */

    P.NationalID,

    P.FirstName,
    P.SecondName,
    P.ThirdName,
    P.LastName,

    CONCAT(
        P.FirstName, N' ',
        P.SecondName, N' ',
        COALESCE(P.ThirdName + N' ', N''),
        P.LastName
    ) AS FullName,

    P.MotherName,

    P.DateOfBirth,

    P.Phone,
    P.AltPhone,

    P.Gender,

    P.Email,

    P.NationalityCountryCode,

    P.ImagePath,

    P.IsActive AS IsPersonActive,

    P.CreatedAt AS PersonCreatedAt,


    /* =====================================================
       Roles
       ===================================================== */

    STRING_AGG(R.Name, N', ') AS Roles

FROM dbo.Users AS U

INNER JOIN dbo.People AS P
    ON P.PersonID = U.PersonID

LEFT JOIN dbo.UserRoles AS UR
    ON UR.UserID = U.UserID

LEFT JOIN dbo.Roles AS R
    ON R.RoleID = UR.RoleID

GROUP BY

    /* User */

    U.UserID,
    U.PersonID,
    U.Username,

    U.IsActive,
    U.IsLocked,

    U.FailedLoginAttempts,
    U.LockedUntil,
    U.LastLoginAt,
    U.PasswordChangedAt,

    U.CreatedAt,
    U.UpdatedAt,


    /* Person */

    P.NationalID,

    P.FirstName,
    P.SecondName,
    P.ThirdName,
    P.LastName,

    P.MotherName,

    P.DateOfBirth,

    P.Phone,
    P.AltPhone,

    P.Gender,

    P.Email,

    P.NationalityCountryCode,

    P.ImagePath,

    P.IsActive,

    P.CreatedAt;
GO
