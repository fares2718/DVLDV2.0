# Sprint 5 – New License Issuance (End-to-End)

**Duration**: 2 weeks  
**Depends on**: Sprint 4

## Goals

Close the complete “First Time License” happy path and main failure paths.

## User Stories / Tasks

1. After all three tests are passed, employee can issue the License.
2. System creates the License with correct IssueDate, ExpirationDate, IssueReason = New.
3. If this is the first license for the Person, a Driver record is created.
4. Application status becomes Completed.
5. All validations (age, hierarchy, no duplicate class) are enforced one last time at issuance.
6. Inquiry shows the newly issued license.

## Technical Deliverables

- License issuance domain service
- End-to-end automated test for the full New License flow
- UI/API for the issuance step

## Definition of Done

- A complete new license can be issued from scratch through the system
- Hierarchical privileges are correctly reflected after issuance