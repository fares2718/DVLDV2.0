export interface CreatePerson {
  nationalId: string;
  firstName: string;
  secondName: string;
  thirdName?: string | null;
  lastName: string;
  motherName: string;
  dateOfBirth: string; // YYYY-MM-DD
  phone: string;
  gender: boolean;
  email: string;
  nationalityCountryCode: string;
  altPhone?: string | null;
}
