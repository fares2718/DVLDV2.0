# Sprint 0 – Foundation & Domain Finalisation

**Duration**: 1–2 weeks  
**Goal**: Remove all major ambiguities before coding starts.

## Objectives

1. Confirm the hierarchical license class model (PrivilegeLevel vs Explicit Inclusion).
2. Finalise the list of Application Statuses.
3. Define exact Practical test fees per class.
4. Produce first version of database schema.
5. Set up repository, CI, coding standards, and project structure.

## Key Decisions to Close

| Decision                   | Options                                  | Recommended                  | Owner                     |
| -------------------------- | ---------------------------------------- | ---------------------------- | ------------------------- |
| Hierarchy model            | A) PrivilegeLevel  B) Explicit Inclusion | Start with A, design for B   | Product Owner + Tech Lead |
| Practical fees             | Need table                               | Must be provided by business | Business                  |
| Application statuses       | See workflows.md                         | Agree final list             | Product Owner             |
| Soft delete vs Hard delete | Soft for most entities                   | Soft                         | Tech Lead                 |
| Driver as separate table   | Yes / No                                 | Yes                          | Tech Lead                 |

## Deliverables

- [x] Signed-off `license-system.md`
- [x] Final `business-rules.md` (no major open questions left)
- [x] Database schema diagram + SQL scripts (v0.1)
- [x] Technology Decision Record (ADR)
- [x] Empty solution with Clean Architecture folders + basic CI
- [x] Sprint 1 ready to start

## Exit Criteria

All items in “Key Decisions” are closed or explicitly deferred with a date.