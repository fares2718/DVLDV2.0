# Business Rules – DVLD

All rules below are derived from the original requirements document + redesign decisions.  
Rules marked with **[NEW]** are introduced by the hierarchical redesign or modernisation.

---

## 1. Person Rules

- BR-P01: NationalID must be unique across the entire system.
- BR-P02: A Person cannot be deleted if he has any Application, License, or is linked to a User (Soft delete instead).
- BR-P03: Once any License is issued to a Person, a Driver record is created (once) and a permanent DriverNumber is assigned.
- BR-P04: Photo is mandatory when issuing the first license.

---

## 2. Application Rules

- BR-A01: Every service starts with creating an Application of the corresponding ApplicationType and paying the Application Fees.
- BR-A02: Application Status starts as “New”.
- BR-A03: An Application can be cancelled only if it is not yet Completed.
- BR-A04: For “New Local License” applications:
  - Applicant must not already have an active license of the same class (or a higher class that includes it – **[NEW]** decision needed).
  - Applicant must not have another incomplete application of the same type + class.
- BR-A05: Application Fees are recorded at creation time (currently fixed per type).

---

## 3. License Class & Hierarchy Rules

- BR-LC01: Each LicenseClass has MinimumAllowedAge, DefaultValidityYears, and ClassFees.
- BR-LC02: Age of applicant is calculated from DateOfBirth on the day of application (or day of issuance – to be confirmed).
- BR-LC03: System must reject application if age < MinimumAllowedAge of the requested class.
- BR-LC04: **[NEW]** Holding a license of class X automatically grants driving privileges for all classes included by X according to the hierarchy.
- BR-LC05: A person may hold licenses from different hierarchy branches (e.g. Motorcycle + Car).

---

## 4. New Local License Issuance Rules

Sequence of tests is mandatory and ordered:

1. Vision Test
2. Theory (Written) Test
3. Practical Test

- BR-NL01: Vision Test must be passed before Theory can be scheduled.
- BR-NL02: Theory must be passed before Practical can be scheduled.
- BR-NL03: Each test requires a paid appointment.
- BR-NL04: If a test is failed, the applicant must create a new appointment (and pay again) via “Retake Test” service or directly.
- BR-NL05: Vision Test fees = 10 USD (fixed in original).
- BR-NL06: Theory Test fees = 20 USD (fixed).
- BR-NL07: Practical Test fees depend on the License Class (exact values not specified in original document).
- BR-NL08: After all three tests are passed, a License can be issued.
- BR-NL09: Issued License has:
  - IssueReason = “New”
  - ExpirationDate = IssueDate + ValidityYears of the class
  - IsActive = true

---

## 5. Retake Test Rules

- BR-RT01: Only allowed if the previous test result was Fail.
- BR-RT02: Creates a new Application of type “Retake Test” linked to the original application.
- BR-RT03: Application fees = 5 USD + the fees of the specific test.
- BR-RT04: Cannot have two open appointments for the same test type on the same application.

---

## 6. Renew License Rules

- BR-RN01: Application fees = 10 USD.
- BR-RN02: Applicant must pass a Vision Test.
- BR-RN03: Old license must be surrendered (marked as replaced / inactive).
- BR-RN04: New license is issued with IssueReason = “Renew” and new expiration date.

---

## 7. Replace Lost License

- BR-RL01: Application fees = 20 USD.
- BR-RL02: System must verify that the license is **not currently detained**.
- BR-RL03: Old license is marked inactive / replaced.
- BR-RL04: New license issued with IssueReason = “Replacement for Lost”.

---

## 8. Replace Damaged License

- BR-RD01: Application fees = 20 USD.
- BR-RD02: Damaged license must be surrendered.
- BR-RD03: System should record the date of replacement for damaged.
- BR-RD04: New license issued with IssueReason = “Replacement for Damaged”.

---

## 9. Detention Rules

- BR-DT01: A license can be detained with: DetainDate, FineFees, Reason, DetainedByUser.
- BR-DT02: While detained, the license cannot be used for Replacement of Lost (and possibly other services).
- BR-DT03: Release requires:
  - Payment of the fine
  - Creating a “Release Detained License” application
  - Recording ReleaseDate and ReleasedByUser

---

## 10. International License Rules

- BR-IL01: Only available to holders of an active Ordinary Car license (Class 3) that is not expired and not detained.
- BR-IL02: Application fees = 20 USD.
- BR-IL03: Validity period is configurable in the system.
- BR-IL04: If an active International License already exists, it must be cancelled/replaced before issuing a new one.
- BR-IL05: History of all previously issued International Licenses must be kept.

---

## 11. General System Rules

- BR-G01: Every create / update / delete operation must write an AuditLog entry containing User, Timestamp, Action, and changed data.
- BR-G02: Soft-delete is preferred over hard-delete for master and transactional data.
- BR-G03: Fees are currently recorded as paid at the moment of creating the application/appointment (no real payment gateway in Phase 1).

---

## 12. Rules That Need Confirmation

| ID  | Rule / Question                                                                | Current Assumption           |
| --- | ------------------------------------------------------------------------------ | ---------------------------- |
| Q1  | Does a higher class automatically prevent applying for a lower included class? | Yes                          |
| Q2  | Exact Practical test fees per class                                            | Unknown – must be defined    |
| Q3  | Is Vision test required on every renewal?                                      | Yes (as per original)        |
| Q4  | Can a detained license be renewed?                                             | No                           |
| Q5  | Exact status values of Application                                             | To be finalised in workflows |
