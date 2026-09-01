# License Class System – Hierarchical Redesign

This is the most critical redesign point of the project.

## 1. Problem with Current Design

In the original system the 7 license classes are completely independent:

1. Small Motorcycle
2. Heavy Motorcycle
3. Ordinary Car
4. Taxi / Limousine
5. Agricultural Vehicles
6. Small/Medium Buses
7. Trucks & Heavy Vehicles

**Consequence**: Holding a Taxi license does **not** automatically grant the right to drive a private car, which is incorrect in real-world regulations in most countries.

## 2. Desired Behavior 

> “The person who holds a taxi/public transport license must be able to drive a private car as well, while holding a private car license does not mean the ability to drive a taxi.”

We need a **privilege inclusion** (hierarchical or set-based) model.

## 3. Proposed Hierarchical Model

We introduce the concept of **Privilege Level** or **Included Classes**.

### 3.1 LicenseClass — Bitwise Privilege Hierarchy

`LicenseClass` represents the different local driving license categories.

The system uses a bitwise privilege model. Each basic driving privilege is represented by one unique bit.

`PrivilegeLevel` stores the **complete privilege mask** granted by the license, including all lower privileges inherited through the hierarchy.

### Privilege Bits

| Privilege               |  Bit |
| ----------------------- | ---: |
| Small Motorcycle        |  `1` |
| Heavy Motorcycle        |  `2` |
| Ordinary Car            |  `4` |
| Taxi / Limousine        |  `8` |
| Agricultural            | `16` |
| Small / Medium Bus      | `32` |
| Heavy Truck / Large Bus | `64` |

These values are powers of two so that individual privileges can be tested using SQL Server's bitwise `&` operator.

### License Hierarchy

The hierarchy represents which higher license grants the privileges of a lower license.

```text
Heavy Motorcycle
        │
        ▼
Small Motorcycle
```

```text
Heavy Truck / Large Bus
        │
        ▼
Small / Medium Bus
        │
        ▼
Taxi / Limousine
        │
        ▼
Ordinary Car
```

```text
Agricultural
    │
    └── Independent
```

`ParentClassID` points from the lower license to the superior license.

For example:

```text
Small Motorcycle.ParentClassID = Heavy Motorcycle
Ordinary Car.ParentClassID     = Taxi
Taxi.ParentClassID             = Small / Medium Bus
Small / Medium Bus.ParentClassID = Heavy Truck
```

### Complete Privilege Masks

Because `PrivilegeLevel` stores all privileges granted by the license:

| License Class           | Own Bit | Complete Privilege Mask |
| ----------------------- | ------: | ----------------------: |
| Small Motorcycle        |     `1` |                     `1` |
| Heavy Motorcycle        |     `2` |                     `3` |
| Ordinary Car            |     `4` |                     `4` |
| Taxi / Limousine        |     `8` |                    `12` |
| Agricultural            |    `16` |                    `16` |
| Small / Medium Bus      |    `32` |                    `44` |
| Heavy Truck / Large Bus |    `64` |                   `108` |

The masks are calculated as:

```text
Heavy Motorcycle
2 + 1 = 3

Taxi / Limousine
8 + 4 = 12

Small / Medium Bus
32 + 8 + 4 = 44

Heavy Truck / Large Bus
64 + 32 + 8 + 4 = 108
```

### Privilege Checking

A driver's effective privilege mask can be checked against a required privilege using the bitwise `&` operator.

```sql
(DriverPrivilegeMask & RequiredPrivilege) = RequiredPrivilege
```

For example, a driver with:

```text
DriverPrivilegeMask = 108
```

can drive:

```text
108 & 64 = 64   → Heavy Truck / Large Bus
108 & 32 = 32   → Small / Medium Bus
108 & 8  = 8    → Taxi / Limousine
108 & 4  = 4    → Ordinary Car
108 & 16 = 0    → Agricultural
```

Therefore, the driver has all privileges in the Heavy Truck hierarchy but does not have the Agricultural privilege.

### Design Rules

1. Each basic privilege must have a unique power-of-two bit.
    
2. `PrivilegeLevel` stores the complete privilege mask, not a numerical rank.
    
3. `ParentClassID` points to the superior license.
    
4. A higher license contains the privileges of all lower licenses in its hierarchy.
    
5. Privileges are checked using the SQL Server `&` operator.
    
6. Agricultural and motorcycle privileges remain independent from the car/bus hierarchy.
    
7. `PrivilegeLevel` uses `TINYINT` because the current system requires only seven bits.
## 4. Business Rules for Hierarchical Classes

1. When issuing a **new** license of class X:
   - Applicant must meet MinimumAllowedAge of X.
   - Applicant must **not** already hold an active license of class X (or higher that already includes X – decision needed).
   - After issuance, the driver is considered authorized for X and all classes included by X.

2. A driver can hold multiple licenses of different branches (Motorcycle + Car).

3. When checking “Does this driver have the right to drive vehicle type Y?”:
   - Look at all active licenses of the driver.
   - If any of them includes Y (via hierarchy), then Yes.

4. International License is currently restricted to holders of Class 3 (Ordinary Car) only. This rule should be reviewed under the new hierarchy.

## 5. Data Model Changes Required

**LicenseClass table additions**:
- PrivilegeLevel INT NULL
- ParentClassID INT NULL (for simple tree)
- IsBaseClass BIT

**License table**: remains mostly the same.  
Authorization logic moves to a domain service / query.
