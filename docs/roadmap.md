# Project Roadmap – DVLD Rebuild

## Overall Phases

| Phase       | Name                       | Goal                                                                          | Target Users            | Duration Estimate |
| ----------- | -------------------------- | ----------------------------------------------------------------------------- | ----------------------- | ----------------- |
| **Phase 1** | Department Web Application | Replace desktop app with a modern internal web system + hierarchical licenses | Employees & Admins only | -                 |
| **Phase 2** | Driver Web Portal          | Self-service for citizens (applications, tracking, documents)                 | Drivers + Employees     | -                 |
| **Phase 3** | Mobile Application         | Native/hybrid mobile experience for the most used driver functions            | Drivers                 | -                 |

**Critical Rule**: Phase 2 and Phase 3 must **not** start until Phase 1 is stable and the hierarchical license model is production-ready.

---

## Phase 1 – Department Web Application (MVP Definition)

### Must-have for Phase 1 MVP
- Full Person & Driver management
- Hierarchical License Class system (PrivilegeLevel or Inclusion)
- All 7 original services working end-to-end
- Test appointment & result recording
- License issuance, renewal, replacement, detention
- International License
- User management + basic roles
- Full audit trail
- Inquiry screens
- Configurable fees & validity periods

### Nice-to-have (can move to later sprints inside Phase 1)
- Advanced reporting & dashboards
- Document upload & storage
- Email/SMS notifications
- Multi-branch support

---

## Sprint Structure (Phase 1)

We work in 2-week sprints.

### Sprint 0 – Foundation (Preparation)
**Goal**: Project setup + final domain decisions  
**Deliverables**:
- Final confirmation of hierarchical license model
- Technology stack decision
- Database schema v1 (including hierarchy)
- CI/CD pipeline skeleton
- Documentation reviewed & signed off

### Sprint 1 – Core Domain & Identity
**Goal**: Person, Driver, User, Authentication, basic CRUD  
**Deliverables**:
- Person management (search by NationalID, create, edit)
- Driver number generation
- User management + login
- Role & permission skeleton
- Audit log infrastructure

### Sprint 2 – License Classes & Hierarchy
**Goal**: Implement the new hierarchical model  
**Deliverables**:
- LicenseClass CRUD + hierarchy configuration
- Privilege calculation service
- “Does driver have privilege for class X?” query
- Seed data for the 7 classes with hierarchy

### Sprint 3 – Application Core
**Goal**: Application entity + ApplicationTypes + basic workflow engine  
**Deliverables**:
- Application CRUD
- ApplicationType management
- Status machine
- Fee recording
- Link Application ↔ Person

### Sprint 4 – New License Flow (Part 1 – Tests)
**Goal**: Vision / Theory / Practical appointment & result  
**Deliverables**:
- TestType management
- TestAppointment creation & scheduling
- Result recording (Pass/Fail)
- Sequence enforcement
- Retake Test service

### Sprint 5 – New License Flow (Part 2 – Issuance)
**Goal**: Complete first-time license issuance  
**Deliverables**:
- Full validation before issuance
- License creation
- Driver creation on first license
- End-to-end “New Local License” happy path + main failure paths

### Sprint 6 – Renew, Replace Lost, Replace Damaged
**Goal**: The three replacement/renewal services  
**Deliverables**:
- Renew workflow (with Vision test)
- Replace Lost (with detention check)
- Replace Damaged
- Old license inactivation logic

### Sprint 7 – Detention & International License
**Goal**: Remaining core services  
**Deliverables**:
- Detain / Release workflows
- International License issuance & history
- Inquiry screens (by NationalID & LicenseNumber)

### Sprint 8 – Hardening, Permissions, Reporting
**Goal**: Production readiness for internal use  
**Deliverables**:
- Fine-grained permissions on all screens
- Basic operational reports
- Performance & security review
- User acceptance testing (UAT) with department staff
- Bug fixing

### Sprint 9+ – Stabilisation & Optional Features
- Document management
- Notifications
- Advanced dashboards
- Performance tuning
- Preparation for Phase 2

---

## Phase 2 – Driver Web Portal (High-Level)

**Prerequisites**: Phase 1 stable + clear API surface.

**MVP of Phase 2**:
- Driver authentication (NationalID + OTP or password)
- View personal data & all licenses
- Submit selected applications online (New License, Renew, Replace Lost/Damaged, International)
- Upload required documents
- Track application status
- View test appointments
- Employees continue to process everything from the internal app

**Out of scope for early Phase 2**:
- Online payment (can be added later)
- Full test booking by driver (may stay employee-driven initially)

---

## Phase 3 – Mobile Application

**Decision criteria** (to be analysed after Phase 2):
Which functions give the highest value on mobile?
Typical candidates:
- View licenses & QR code for police
- Track application status
- Receive push notifications
- Simple document upload
- Appointment reminders

Avoid simply copying every web page.

---

## Dependencies & Risks

| Dependency / Risk                 | Mitigation                                   |
| --------------------------------- | -------------------------------------------- |
| Final hierarchy rules not decided | Sprint 0 must close this                     |
| Practical test fees unknown       | Define before Sprint 4                       |
| Change of scope from department   | Freeze core domain early                     |
| Parallel development of portal    | Strict API versioning + feature flags        |
| Data migration from old desktop   | Separate migration project after Phase 1 MVP |

---

## Success Criteria for Phase 1

- All original services work correctly under the new hierarchical model
- Zero critical bugs after 2 weeks of parallel run with old system (if possible)
- Department staff can perform daily work without the old desktop application
- Full audit trail available for every action
- Documentation is complete and up-to-date