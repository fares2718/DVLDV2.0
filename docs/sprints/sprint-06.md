# Sprint 6 – Renew & Replacements

**Duration**: 2 weeks  
**Depends on**: Sprint 5

## Goals

Implement Renew, Replace Lost, and Replace Damaged services.

## User Stories / Tasks

1. Renew: create application → Vision test → surrender old license → issue new license.
2. Replace Lost: validate not detained → create application → issue new license → deactivate old.
3. Replace Damaged: create application → issue new license → deactivate old + record replacement date.
4. Old licenses are properly marked inactive / replaced.
5. IssueReason is set correctly on the new license.

## Technical Deliverables

- Three complete workflows
- Shared “Issue Replacement License” logic
- Tests covering detention blocking for Replace Lost

## Definition of Done

- All three services work end-to-end
- License history is correctly maintained