import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { LoginRequest } from '../features/auth/models/login-request';
import { CurrentUser } from '../features/auth/models/current-user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient) {}

  login(request: LoginRequest) {
    return this.http.post<void>(`${this.apiUrl}/login`, request, { withCredentials: true });
  }

  currentUser() {
    return this.http.get<CurrentUser>(`${this.apiUrl}/current-user`, { withCredentials: true });
  }

  refresh() {
    return this.http.post<void>(`${this.apiUrl}/refresh`, {}, { withCredentials: true });
  }

  logout() {
    return this.http.post<void>(`${this.apiUrl}/logout`, {}, { withCredentials: true });
  }
}
