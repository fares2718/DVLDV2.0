import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Address } from '../../features/people/models/address';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class AddressService {
  private http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getPersonAddresses(personId: string): Observable<Address[]> {
    return this.http.get<Address[]>(`${this.apiUrl}/Address/get-person-addresses/${personId}`, {
      withCredentials: true,
    });
  }
}
