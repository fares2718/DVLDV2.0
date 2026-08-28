# Workflows – DVLD

## 1. New Local Driving License (First Time)

```
1. Employee searches Person by NationalID
   └─ If not found → Create new Person
2. Create Application (Type = New Local License)
   - Select LicenseClass
   - System validates:
     • Age ≥ MinimumAllowedAge
     • No active license of same (or included) class
     • No open application of same type+class
   - Collect Application Fees (5 USD)
3. Schedule & Conduct Vision Test
   - Create TestAppointment (Vision)
   - Pay 10 USD
   - Record Result (Pass/Fail)
   - If Fail → stop (must retake later)
4. Schedule & Conduct Theory Test
   - Only if Vision = Pass
   - Pay 20 USD
   - Record Result + Score
   - If Fail → stop
5. Schedule & Conduct Practical Test
   - Only if Theory = Pass
   - Pay Practical fees (class-dependent)
   - Record Result
   - If Fail → stop
6. Issue License
   - All tests passed
   - Create License record
   - Create Driver record if first license
   - Set Application Status = Completed
```

## 2. Retake Test

```
1. Find original Application / previous failed Test
2. Create new Application (Type = Retake Test) linked to original
3. Pay Application Fees (5) + Test Fees
4. Create new TestAppointment for the failed TestType
5. Record new Result
```

## 3. Renew License

```
1. Find active License by LicenseNumber or NationalID
2. Validate: not detained, not already expired beyond grace? (grace period not defined)
3. Create Application (Type = Renew)
4. Collect 10 USD
5. Schedule & Pass Vision Test
6. Surrender old license (mark inactive)
7. Issue new License (IssueReason = Renew, new ExpirationDate)
```

## 4. Replace Lost License

```
1. Find License
2. Validate: IsActive = true AND not currently Detained
3. Create Application (Type = Replace Lost)
4. Collect 20 USD
5. Mark old License as inactive / replaced
6. Issue new License (IssueReason = Replacement for Lost)
```

## 5. Replace Damaged License

```
1. Find License
2. Create Application (Type = Replace Damaged)
3. Collect 20 USD
4. Mark old as replaced
5. Issue new License (IssueReason = Replacement for Damaged)
6. Record replacement date
```

## 6. Detain License

```
1. Find active License
2. Create Detention record
   - Fine amount
   - Reason
   - DetainDate
3. License is now considered detained
```

## 7. Release Detained License

```
1. Find Detention record
2. Create Application (Type = Release Detained)
3. Collect fine + application fees (if any)
4. Mark Detention as Released
5. Record ReleaseDate & User
```

## 8. Issue International License

```
1. Find Driver who has active Class 3 (Ordinary Car) license
2. Validate: local license not expired and not detained
3. Check if active International License exists → cancel it if needed
4. Create Application
5. Collect 20 USD
6. Issue International License with configurable validity
```

## 9. Inquiry

- Search by NationalID → list of all Licenses + Applications + Detentions
- Search by LicenseNumber → full license details + history

---

## Status Transitions (Application)

Suggested state machine (to be refined):

```
New
  → Under Processing
  → Waiting for Vision
  → Waiting for Theory
  → Waiting for Practical
  → Ready for Issuance
  → Completed
  → Cancelled
  → Rejected
```

Each transition must be logged.