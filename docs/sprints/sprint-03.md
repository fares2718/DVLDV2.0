# Sprint 3 – Application Core

**Duration**: 2 weeks  
**Depends on**: Sprint 1 & 2

## Goals

Create the central Application entity and the configurable Application Types.

## User Stories / Tasks

1. Admin can manage Application Types and their base fees.
2. Employee can create an Application for a Person and choose Type + (when relevant) LicenseClass.
3. System validates basic rules at creation time (age, existing open applications, etc.).
4. Application has a clear status and status history.
5. Fees paid are recorded on the Application.
6. Applications can be searched by Application Number or NationalID.

## Technical Deliverables

- Application + ApplicationType aggregates
- Status state machine (at least the main happy path)
- Validation service that uses the License Privilege service
- Basic list + detail screens / APIs

## Definition of Done

- New Local License application can be created and validated
- Other application types can be created (even if their full workflow is not yet implemented)