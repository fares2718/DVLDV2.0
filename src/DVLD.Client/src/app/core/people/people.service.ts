import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpParams } from '@angular/common/http';
import { tap, type Observable } from 'rxjs';
import { PagedList } from '../../shared/models/paged-list';
import { PersonSummary } from '../../features/people/models/person-summary';
import { type PeopleFilter } from '../../features/people/models/people-filter';
import { CreatePerson } from '../../features/people/models/create-person';

@Injectable({
  providedIn: 'root',
})
export class PeopleService {
  private readonly apiUrl = `${environment.apiUrl}/Person`;
  private readonly http = inject(HttpClient);

  private readonly _peopleSummary = signal<PagedList<PersonSummary> | null>(null);
  readonly peopleSummary = this._peopleSummary.asReadonly();

  count: number = 0;

  createPerson(person: CreatePerson) {
    return this.http.post(`${this.apiUrl}/create-person`, person, {
      withCredentials: true,
    });
  }

  updatePersonName(
    personId: string,
    name: {
      firstName: string;
      secondName: string;
      thirdName: string;
      lastName: string;
      motherName: string;
    },
  ): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/update-person-name/${personId}`, name, {
      withCredentials: true,
    });
  }

  updatePersonContactInfo(
    personId: string,
    contactInfo: { phone: string; altPhone: string },
  ): Observable<void> {
    return this.http.patch<void>(
      `${this.apiUrl}/update-person-contact-info/${personId}`,
      contactInfo,
      {
        withCredentials: true,
      },
    );
  }

  updatePersonalInfo(
    personId: string,
    personalInfo: { dateOfBirth: string; nationalityCountryCode: string },
  ): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/update-personal-info/${personId}`, personalInfo, {
      withCredentials: true,
    });
  }

  uploadPersonImage(personId: string, image: File, personName: string): Observable<void> {
    const extension = image.name.includes('.') ? image.name.slice(image.name.lastIndexOf('.')) : '';
    const fileName = `${personName.trim().replace(/[^a-zA-Z0-9]+/g, '_')}${extension}`;
    const formData = new FormData();
    formData.append('FileName', fileName);
    formData.append('image', image, fileName);

    return this.http.patch<void>(`${this.apiUrl}/upload-image/${personId}`, formData, {
      withCredentials: true,
    });
  }

  getPeopleCount(): Observable<number> {
    return this.http
      .get<number>(`${this.apiUrl}/get-people-count`, { withCredentials: true })
      .pipe(tap((count) => (this.count = count)));
  }

  getPeopleSummary(filters: PeopleFilter): Observable<PagedList<PersonSummary>> {
    let params = new HttpParams();
    const optionalFilters = {
      search: filters.search,
      nationalId: filters.nationalId,
      name: filters.name,
      gender: filters.gender,
      phone: filters.phone,
      email: filters.email,
      sortBy: filters.sortBy,
      isDescending: filters.isDescending,
      isActive: filters.isActive,
    };

    for (const [key, value] of Object.entries(optionalFilters)) {
      if (value !== null && value !== undefined) {
        params = params.set(key, value.toString());
      }
    }

    params = params.set('pageNumber', (filters.pageNumber ?? 1).toString());
    params = params.set('pageSize', (filters.pageSize ?? 10).toString());

    return this.http
      .get<PagedList<PersonSummary>>(`${this.apiUrl}/filter-people`, {
        params,
        withCredentials: true,
      })
      .pipe(tap((peopleSummary) => this._peopleSummary.set(peopleSummary)));
  }

  getPersonById(personId: string): Observable<PersonSummary> {
    return this.http.get<PersonSummary>(`${this.apiUrl}/get-person/${personId}`, {
      withCredentials: true,
    });
  }

  activatePerson(personId: string): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/activate-person/${personId}`, null, {
      withCredentials: true,
      responseType: 'text',
    });
  }

  deactivatePerson(personId: string): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/deactivate-person/${personId}`, null, {
      withCredentials: true,
      responseType: 'text',
    });
  }
}
