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

  createPerson(person: CreatePerson) {
    return this.http.post(`${this.apiUrl}/create-person`, person, {
      withCredentials: true,
    });
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
}
