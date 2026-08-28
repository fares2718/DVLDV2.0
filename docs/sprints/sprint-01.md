# Sprint 1 – Core Domain & Identity

**Duration**: 2 weeks  
**Depends on**: Sprint 0 completed

## Goals

- Implement Person and Driver management
- Implement User authentication and basic authorisation
- Establish AuditLog infrastructure
- Create the foundation that every later feature will use

## User Stories / Tasks

1. As an employee I can search for a Person by NationalID.
2. As an employee I can create a new Person with all required fields.
3. As an employee I can edit Person data (with audit).
4. When the first license is issued, a Driver record and DriverNumber are created automatically.
5. System Administrator can create Users linked to existing Persons.
6. Users can log in with Username + Password.
7. Every create/update is written to AuditLog.
8. Basic role system (Admin, Officer, ReadOnly) exists.

## Technical Deliverables

- Person aggregate + repository
- Driver service (number generation strategy)
- Identity / Authentication module
- Audit interceptor or domain event handler
- Unit + integration tests for the above
- Seed data for a few test persons

## Definition of Done

- All stories above are demoable
- Code reviewed
- Tests green
- Documentation updated if any rule changed