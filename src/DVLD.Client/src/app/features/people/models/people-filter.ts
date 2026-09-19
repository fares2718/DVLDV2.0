export type PeopleFilter = {
  search?: string | null;
  nationalId?: string | null;
  name?: string | null;
  gender?: string | null;
  phone?: string | null;
  email?: string | null;
  sortBy?: string | null;
  isDescending: boolean;
  isActive?: boolean | null;
  pageNumber?: number;
  pageSize?: number;
};
