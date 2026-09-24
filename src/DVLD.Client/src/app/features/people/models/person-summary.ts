export interface PersonSummary {
  personId: string;
  nationalId: string;
  fullName: string;
  firstName?: string;
  secondName?: string;
  thirdName?: string | null;
  lastName?: string;
  motherName: string;
  dateOfBirth: Date;
  phone: string;
  altPhone?: string;
  email: string;
  gender: string;
  nationality: string;
  isActive: boolean;
  imagePath?: string | null;
}
