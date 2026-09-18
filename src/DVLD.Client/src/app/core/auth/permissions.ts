export enum Permission {
  None = 0,

  // People
  ViewPeople = 1 << 0,
  CreatePeople = 1 << 1,
  EditPeople = 1 << 2,

  // Applications
  ViewApplications = 1 << 3,
  CreateApplications = 1 << 4,
  EditApplications = 1 << 5,
  CancelApplications = 1 << 6,

  // Licenses
  ViewLicenses = 1 << 7,
  IssueLicenses = 1 << 8,
  RenewLicenses = 1 << 9,
  ReplaceLicenses = 1 << 10,

  // Tests
  ViewTests = 1 << 11,
  ScheduleTests = 1 << 12,
  RecordTestResults = 1 << 13,

  // Detention
  ViewDetentions = 1 << 14,
  DetainLicenses = 1 << 15,
  ReleaseLicenses = 1 << 16,
  ManageFines = 1 << 17,

  // Users & Roles
  ViewUsers = 1 << 18,
  ManageUsers = 1 << 19,
  ViewRoles = 1 << 20,
  ManageRoles = 1 << 21,

  // Configuration / Master Data
  ViewMasterData = 1 << 22,
  ManageMasterData = 1 << 23,

  // Audit
  ViewAuditLogs = 1 << 24,

  // Driver self-service
  ViewOwnData = 1 << 25,
  SubmitApplications = 1 << 26,
  UploadDocuments = 1 << 27,
  TrackOwnApplications = 1 << 28,
}
