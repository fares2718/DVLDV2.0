# DVLD – Driving & Vehicle License Department

## Project Rebuilding Documentation

**Project**: DVLD – System 2: Driver License Issuance & Management  
**Goal**: Full redesign and rebuild of the existing desktop application into a modern, scalable multi-channel system.

### Vision

Transform the current single desktop application into a phased platform:

1. **Phase 1** – Internal Web Application for the Department (employees only)
2. **Phase 2** – Driver Web Portal (self-service for citizens/drivers)
3. **Phase 3** – Mobile Application (self-service for citizens/drivers)

### Documentation Structure

```
docs/
├── README.md                 ← This file
├── system-analysis.md        ← Actors, roles, entities
├── domain-analysis.md        ← Core domain model & relationships
├── business-rules.md         ← All business rules & constraints
├── workflows.md              ← End-to-end process flows
├── license-system.md         ← Hierarchical license class design (critical redesign)
├── roadmap.md                ← Overall roadmap, phases, MVP definitions
└── sprints/
    ├── sprint-01.md
    ├── sprint-02.md
    └── ...
```

### Key Redesign Drivers

| Problem in Current System                 | Redesign Decision                                  |
| ----------------------------------------- | -------------------------------------------------- |
| Everything happens inside one Desktop app | Move to Web Application (Phase 1) + later portals  |
| License classes are flat & independent    | Introduce hierarchical / privilege-inclusion model |
| No self-service for citizens              | Add Driver Portal (Phase 2) and Mobile (Phase 3)   |
| Limited auditability                      | Full audit trail on every action                   |
| Hard to extend                            | Clean domain model + configurable rules            |

### How to Use This Documentation

- Start with `system-analysis.md` and `domain-analysis.md` to understand the domain.
- Read `license-system.md` carefully – this is the most important redesign.
- Use `business-rules.md` and `workflows.md` as the source of truth for implementation.
- Follow `roadmap.md` and the individual sprint files for execution order.