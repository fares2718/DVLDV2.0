export interface Address {
  addressId: string;
  personId: string;
  addressType: string;
  countryCode: string;
  governorate: string;
  city: string;
  street: string;
  buildingNumber: string;
  apartmentNumber: string | null;
  postalCode: string | null;
  additionalDetails: string | null;
  isPrimary: boolean;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}
