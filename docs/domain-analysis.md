# Domain Analysis – DVLD

## 1. Core Domain Entities

### 1.1 Person

Represents any individual known to the system (applicant, driver, or system user).

**Key Attributes**
- PersonID (GUID,Primary data key)
- NationalID (unique, primary business key)
- FullName (First/Second/Third/Last Col for each)
- MotherName 
- DateOfBirth
- Address
- Phone
- Alt Phone
- Gender
- Email
- Nationality
- Image (path)
- CreatedDate
- IsActive

**Rules**
- NationalID must be unique.
- A Person can exist without being a Driver.
- Once any License is issued, the Person becomes a formal Driver and receives a DriverNumber (assigned once).

### 1.2 Driver

A specialization or extension of Person.

- DriverID / DriverNumber (system-generated, unique, assigned once)
- PersonID (1:1)
- CreatedDate

All future licenses are linked to this Driver record.

### 1.3 User (System User)

- UserID
- PersonID (mandatory link)
- Username (unique)
- PasswordHash
- IsActive / IsLocked
- Roles (many-to-many)

### 1.4 Application

Central transaction entity.

**Key Attributes**
- ApplicationID
- ApplicantPersonID
- ApplicationTypeID
- ApplicationDate
- Status (New, Cancelled, Completed, …)
- PaidFees
- CreatedByUserID
- LastStatusDate
- RelatedLicenseID (nullable – for renew/replace)
- RelatedApplicationID (nullable – for retake)

### 1.5 ApplicationType

Configurable list of services:

1. New Local Driving License
2. Retake Test
3. Renew Driving License
4. Replace Lost License
5. Replace Damaged License
6. Release Detained License
7. International License

Each type has:
- Title
- BaseFees
- Description
- IsActive

### 1.6 LicenseClass (Redesigned)

See dedicated document `license-system.md`.

**Key Attributes**
- LicenseClassID
- ClassName
- ClassDescription
- MinimumAllowedAge
- DefaultValidityYears
- ClassFees
- Hierarchy / Privilege Level (new)
- ParentClassID or PrivilegeSet (new)

### 1.7 License (Local Driving License)

- LicenseID
- DriverID
- LicenseClassID
- IssueDate
- ExpirationDate
- IsActive
- IssueReason (New, Renew, Replacement for Lost, Replacement for Damaged, …)
- Notes / Conditions
- ApplicationID (the application that produced this license)

A Driver can hold multiple Licenses of different classes (subject to hierarchy rules).

### 1.8 InternationalLicense

- InternationalLicenseID
- DriverID
- IssuedUsingLocalLicenseID
- IssueDate
- ExpirationDate
- IsActive
- ApplicationID

### 1.9 TestType

Fixed types (currently):
1. Vision Test
2. Written (Theory) Test
3. Practical (Street) Test

Attributes: TestTypeID, Title, Description, Fees (base), OrderInSequence.

### 1.10 TestAppointment

- TestAppointmentID
- LocalDrivingLicenseApplicationID (or general Application)
- TestTypeID
- AppointmentDate
- PaidFees
- IsLocked (once result is entered)
- CreatedByUserID

### 1.11 Test (Result)

- TestID
- TestAppointmentID
- TestResult (Pass / Fail)
- Notes
- CreatedByUserID
- CreatedDate

### 1.12 Detention

- DetentionID
- LicenseID
- DetainDate
- FineFees
- CreatedByUserID
- IsReleased
- ReleaseDate
- ReleasedByUserID
- ReleaseApplicationID

### 1.13 AuditLog

Mandatory for every write operation.

- LogID
- UserID
- ActionType
- EntityName
- EntityID
- OldValues / NewValues (JSON)
- Timestamp
- IP / Machine (optional)

---

## 2. Key Relationships

```
Person 1 ── 1 Driver
Person 1 ── * Application
Person 1 ── 1 User (optional)

Driver 1 ── * License
Driver 1 ── * InternationalLicense

Application * ── 1 ApplicationType
Application 1 ── * TestAppointment
Application 1 ── 0..1 License (issued from it)

License * ── 1 LicenseClass
License 1 ── 0..1 Detention (active)

LicenseClass ── hierarchy (self-referencing or privilege table)
```

---

## 3. Application Status Lifecycle (Proposed)

Suggested statuses:

- **New** – just created
- **Pending Payment** (if payment is separate)
- **Under Processing**
- **Waiting for Tests**
- **Tests Completed**
- **Ready for Issuance**
- **Completed**
- **Cancelled**
- **Rejected**

Exact status machine will be refined in workflows.

---
