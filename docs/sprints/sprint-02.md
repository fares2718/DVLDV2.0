# Sprint 2 – License Classes & Hierarchy Engine

**Duration**: 2 weeks  
**Depends on**: Sprint 1

## Goals

Implement the new hierarchical license class system so that the rest of the application can rely on it.

## User Stories / Tasks

1. Admin can view and edit License Classes (Name, Description, MinAge, ValidityYears, Fees, PrivilegeLevel).
2. System correctly calculates whether a Driver has privilege for a given class.
3. When checking “already has this class”, the hierarchy is taken into account.
4. Seed the 7 original classes with agreed hierarchy values.
5. Unit tests cover all hierarchy edge cases.

## Technical Deliverables

- LicenseClass entity + configuration UI (or API)
- `ILicensePrivilegeService` with methods:
  - `bool HasPrivilege(DriverId, LicenseClassId)`
  - `IEnumerable<LicenseClass> GetEffectiveClasses(DriverId)`
- Database migration for hierarchy columns / inclusion table
- Comprehensive test suite for the privilege engine

## Definition of Done

- Privilege service is the single source of truth
- All later features will call this service instead of checking ClassID equality
- Documentation in `license-system.md` matches the implementation