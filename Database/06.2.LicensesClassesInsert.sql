USE DVLD2p0;
GO

/* =========================================================
   License Classes - Seed Data
   ========================================================= */

/*
    Privilege Bits:

    1   = Small Motorcycle
    2   = Heavy Motorcycle
    4   = Ordinary Car
    8   = Taxi / Limousine
    16  = Agricultural
    32  = Small / Medium Bus
    64  = Heavy Truck / Large Bus
*/


/* =========================================================
   1. Heavy Motorcycle
   Privileges: Heavy Motorcycle + Small Motorcycle
   Mask: 2 + 1 = 3
   ========================================================= */

INSERT INTO dbo.LicenseClasses
(
    ClassName,
    ClassDescription,
    MinimumAllowedAge,
    MaximumAllowedAge,
    DefaultValidityYears,
    ClassFees,
    PrivilegeLevel,
    ParentClassID
)
VALUES
    (
        N'Heavy Motorcycle',
        N'License for driving heavy motorcycles and small motorcycles.',
        21,
        60,
        5,
        75.00,
        3,
        NULL
    );
GO


/* =========================================================
   2. Small Motorcycle
   Superior: Heavy Motorcycle
   Mask: 1
   ========================================================= */

INSERT INTO dbo.LicenseClasses
(
    ClassName,
    ClassDescription,
    MinimumAllowedAge,
    MaximumAllowedAge,
    DefaultValidityYears,
    ClassFees,
    PrivilegeLevel,
    ParentClassID
)
SELECT
    N'Small Motorcycle',
    N'License for driving small motorcycles.',
    18,
    60,
    5,
    50.00,
    1,
    LicenseClassID
FROM dbo.LicenseClasses
WHERE ClassName = N'Heavy Motorcycle';
GO


/* =========================================================
   3. Heavy Truck / Large Bus
   Privileges:
       Heavy Truck / Large Bus
       Small / Medium Bus
       Taxi / Limousine
       Ordinary Car

   Mask:
       64 + 32 + 8 + 4 = 108
   ========================================================= */

INSERT INTO dbo.LicenseClasses
(
    ClassName,
    ClassDescription,
    MinimumAllowedAge,
    MaximumAllowedAge,
    DefaultValidityYears,
    ClassFees,
    PrivilegeLevel,
    ParentClassID
)
VALUES
    (
        N'Heavy Truck / Large Bus',
        N'License for driving heavy trucks and large buses, including small and medium buses, taxis, and ordinary cars.',
        21,
        70,
        10,
        250.00,
        108,
        NULL
    );
GO


/* =========================================================
   4. Small / Medium Bus
   Superior: Heavy Truck / Large Bus

   Privileges:
       Small / Medium Bus
       Taxi / Limousine
       Ordinary Car

   Mask:
       32 + 8 + 4 = 44
   ========================================================= */

INSERT INTO dbo.LicenseClasses
(
    ClassName,
    ClassDescription,
    MinimumAllowedAge,
    MaximumAllowedAge,
    DefaultValidityYears,
    ClassFees,
    PrivilegeLevel,
    ParentClassID
)
SELECT
    N'Small / Medium Bus',
    N'License for driving small and medium buses, taxis, and ordinary cars.',
    21,
    70,
    10,
    180.00,
    44,
    LicenseClassID
FROM dbo.LicenseClasses
WHERE ClassName = N'Heavy Truck / Large Bus';
GO


/* =========================================================
   5. Taxi / Limousine
   Superior: Small / Medium Bus

   Privileges:
       Taxi / Limousine
       Ordinary Car

   Mask:
       8 + 4 = 12
   ========================================================= */

INSERT INTO dbo.LicenseClasses
(
    ClassName,
    ClassDescription,
    MinimumAllowedAge,
    MaximumAllowedAge,
    DefaultValidityYears,
    ClassFees,
    PrivilegeLevel,
    ParentClassID
)
SELECT
    N'Taxi / Limousine',
    N'License for driving taxis, limousines, and ordinary cars.',
    21,
    70,
    10,
    150.00,
    12,
    LicenseClassID
FROM dbo.LicenseClasses
WHERE ClassName = N'Small / Medium Bus';
GO


/* =========================================================
   6. Ordinary Car (Private)
   Superior: Taxi / Limousine

   Mask:
       4
   ========================================================= */

INSERT INTO dbo.LicenseClasses
(
    ClassName,
    ClassDescription,
    MinimumAllowedAge,
    MaximumAllowedAge,
    DefaultValidityYears,
    ClassFees,
    PrivilegeLevel,
    ParentClassID
)
SELECT
    N'Ordinary Car (Private)',
    N'License for driving ordinary private cars.',
    18,
    70,
    10,
    100.00,
    4,
    LicenseClassID
FROM dbo.LicenseClasses
WHERE ClassName = N'Taxi / Limousine';
GO


/* =========================================================
   7. Agricultural
   Independent license
   Mask: 16
   ========================================================= */

INSERT INTO dbo.LicenseClasses
(
    ClassName,
    ClassDescription,
    MinimumAllowedAge,
    MaximumAllowedAge,
    DefaultValidityYears,
    ClassFees,
    PrivilegeLevel,
    ParentClassID
)
VALUES
    (
        N'Agricultural',
        N'License for driving agricultural vehicles.',
        21,
        70,
        10,
        120.00,
        16,
        NULL
    );
GO


/* =========================================================
   Verification
   ========================================================= */

SELECT
    LC.LicenseClassID,
    LC.ClassName,
    LC.PrivilegeLevel,
    Parent.ClassName AS SuperiorClass,
    LC.MinimumAllowedAge,
    LC.MaximumAllowedAge,
    LC.DefaultValidityYears,
    LC.ClassFees,
    LC.IsActive
FROM dbo.LicenseClasses AS LC
         LEFT JOIN dbo.LicenseClasses AS Parent
                   ON Parent.LicenseClassID = LC.ParentClassID
ORDER BY LC.LicenseClassID;
GO