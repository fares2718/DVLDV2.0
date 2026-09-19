export interface PersonSummary {
  personId: string;
  nationalId: string;
  fullName: string;
  motherName: string;
  dateOfBirth: Date;
  phone: string;
  altPhone?: string;
  email: string;
  gender: string;
  nationalityCountryCode: string;
  isActive: boolean;
}
