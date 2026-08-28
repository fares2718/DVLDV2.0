# Sprint 4 – Tests (Appointments & Results)

**Duration**: 2 weeks  
**Depends on**: Sprint 3

## Goals

Implement the three test types, appointments, results, and the retake mechanism.

## User Stories / Tasks

1. Employee can schedule a Vision / Theory / Practical appointment for an application.
2. System enforces the correct order (Vision → Theory → Practical).
3. Employee can record Pass/Fail (and score for Theory).
4. Failed tests block progression.
5. Retake Test service creates a new application linked to the original and allows a new appointment.
6. Fees for each test type are applied correctly.

## Technical Deliverables

- TestType, TestAppointment, Test entities
- Sequence validation service
- Retake Test workflow
- UI/API for scheduling and result entry

## Definition of Done

- Full test cycle for a New License application can be completed (or failed and retaken)