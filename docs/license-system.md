# License Class System – Hierarchical Redesign

This is the most critical redesign point of the project.

## 1. Problem with Current Design

In the original system the 7 license classes are completely independent:

1. Small Motorcycle
2. Heavy Motorcycle
3. Ordinary Car
4. Taxi / Limousine
5. Agricultural Vehicles
6. Small/Medium Buses
7. Trucks & Heavy Vehicles

**Consequence**: Holding a Taxi license does **not** automatically grant the right to drive a private car, which is incorrect in real-world regulations in most countries.

## 2. Desired Behavior 

> “The person who holds a taxi/public transport license must be able to drive a private car as well, while holding a private car license does not mean the ability to drive a taxi.”

We need a **privilege inclusion** (hierarchical or set-based) model.

## 3. Proposed Hierarchical Model

We introduce the concept of **Privilege Level** or **Included Classes**.

### Option A – Simple Numeric Hierarchy (Recommended for first version)

Assign each class a `PrivilegeLevel` (integer).  
A driver is allowed to drive any class whose PrivilegeLevel ≤ the highest level he holds.

| Class | Name                    | Suggested PrivilegeLevel | Min Age | Default Validity | Notes            |
| ----- | ----------------------- | ------------------------ | ------- | ---------------- | ---------------- |
| 1     | Small Motorcycle        | 1                        | 18      | 5 years          | Base motorcycle  |
| 2     | Heavy Motorcycle        | 2                        | 21      | 5 years          | Includes Class 1 |
| 3     | Ordinary Car (Private)  | 10                       | 18      | 10 years         | Base car         |
| 4     | Taxi / Limousine        | 20                       | 21      | 10 years         | Includes Class 3 |
| 5     | Agricultural            | 15                       | 21      | 10 years         | Independent      |
| 6     | Small/Medium Bus        | 30                       | 21      | 10 years         | Includes 3 + 4   |
| 7     | Heavy Truck / Large Bus | 40                       | 21      | 10 years         | Highest          |

**Advantages**: Very simple to implement and query.  
**Disadvantages**: Not flexible enough if the inclusion is not strictly linear (e.g. Agricultural may not include Taxi).

## 4. Business Rules for Hierarchical Classes

1. When issuing a **new** license of class X:
   - Applicant must meet MinimumAllowedAge of X.
   - Applicant must **not** already hold an active license of class X (or higher that already includes X – decision needed).
   - After issuance, the driver is considered authorized for X and all classes included by X.

2. A driver can hold multiple licenses of different branches (Motorcycle + Car).

3. When checking “Does this driver have the right to drive vehicle type Y?”:
   - Look at all active licenses of the driver.
   - If any of them includes Y (via hierarchy), then Yes.

4. International License is currently restricted to holders of Class 3 (Ordinary Car) only. This rule should be reviewed under the new hierarchy.

## 5. Data Model Changes Required

**LicenseClass table additions**:
- PrivilegeLevel INT NULL
- ParentClassID INT NULL (for simple tree)
- IsBaseClass BIT

**License table**: remains mostly the same.  
Authorization logic moves to a domain service / query.
