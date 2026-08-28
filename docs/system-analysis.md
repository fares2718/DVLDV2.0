# System Analysis – DVLD Driver License Management

## 1. Actors & Users

### Primary Actors

| Actor                    | Description                                                                    | Interaction Channel (Target)                   |
| ------------------------ | ------------------------------------------------------------------------------ | ---------------------------------------------- |
| **Department Employee**  | Staff working inside the licensing department. Performs all operational tasks. | Web Application (Phase 1)                      |
| **System Administrator** | Manages users, roles, permissions, configuration, and master data.             | Web Application (Phase 1)                      |
| **Driver / Citizen**     | Person who holds or applies for driving licenses.                              | Driver Web Portal (Phase 2) + Mobile (Phase 3) |
| **Supervisor / Manager** | Reviews sensitive operations, approves exceptions, monitors performance.       | Web Application (Phase 1)                      |

### Secondary / Supporting Actors

- External Medical Examiner (Vision test results may come from outside)
- External Theory Exam Center (results recorded in system)
- External Practical Exam Center
- Payment Gateway 
- National Identity System (for National ID validation – future integration)

> **Note**: In Phase 1 the system is **internal only**. Drivers interact only through employees.

---

## 2. Roles & Permissions (High-Level)

| Role                    | Typical Permissions                                                           |
| ----------------------- | ----------------------------------------------------------------------------- |
| **Admin**               | Full system access, user management, configuration, master data               |
| **License Officer**     | Create/edit applications, issue licenses, handle replacements & renewals      |
| **Test Scheduler**      | Schedule and record test appointments & results                               |
| **Detention Officer**   | Detain / release licenses, manage fines                                       |
| **Read-Only / Inquiry** | Search and view people, licenses, applications                                |
| **Driver (self)**       | View own data, submit applications, upload documents, track status (Phase 2+) |

Permissions should be fine-grained and assigned via roles. A user can have multiple roles.

---

## 3. Core Entities (High-Level)

- **Person** (Citizen / Applicant / Driver)
- **User** (System user – linked to a Person)
- **Application** (Request for a service)
- **LicenseClass** (with hierarchy)
- **License** (issued driving license)
- **InternationalLicense**
- **Test** / **TestAppointment** / **TestResult**
- **Detention** (License detention / seizure)
- **ApplicationType** (service types)
- **ApplicationStatus**
- **AuditLog**

Detailed attributes and relationships are in `domain-analysis.md`.

---

## 4. Main Business Processes (Overview)

1. Person registration / management
2. New Local License Application (with tests)
3. Retake Test
4. Renew License
5. Replace Lost License
6. Replace Damaged License
7. Detain / Release License
8. Issue International License
9. Inquiry (by National ID or License Number)
10. User & Role Management
11. Master Data Management (License Classes, Test Types, Fees, etc.)

---

## 5. What Must Be Redesigned vs. What Can Be Reused

| Area                                    | Current State                      | Decision                                 |
| --------------------------------------- | ---------------------------------- | ---------------------------------------- |
| License Classes                         | Flat list of 7 independent classes | **Full redesign** → Hierarchical model   |
| Application Types & Fees                | Hard-coded / simple                | Keep concept, make configurable          |
| Test Types (Vision, Theory, Practical)  | Fixed sequence                     | Keep sequence, improve modeling          |
| Person uniqueness by National ID        | Exists                             | Keep & strengthen                        |
| License issuance after all tests passed | Exists                             | Keep core flow                           |
| Desktop UI                              | Monolithic                         | Replace completely with Web              |
| User management                         | Basic                              | Redesign with proper roles & permissions |
| Audit trail                             | Partial / missing                  | Make mandatory on every write            |

---

## 6. Missing Information / Assumptions

The following points are **not fully specified** in the original requirements and must be decided:

1. Exact hierarchy rules between the 7 license classes (see `license-system.md`).
2. Whether a person can hold multiple active licenses of the same class (currently forbidden for new issue).
3. Detailed rules for International License validity period and whether it is linked to a specific local license.
4. Exact fee structure for Practical test per class (mentioned as “according to class” but no table given).
5. Whether Vision test is required for every renewal or only under certain conditions.
6. Document management (what documents are required for each service and how they are stored).
7. Payment integration (currently only fee recording).
8. Multi-branch / multi-office support.
9. Language support (Arabic primary, English secondary?).
10. Notification channels (email / SMS) for status changes.

All of the above are treated as **open decisions** .