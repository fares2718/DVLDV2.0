import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { LoginRequest } from '../../features/auth/models/login-request';
import { CurrentUser } from '../../features/auth/models/current-user';
import { catchError, Observable, of, tap } from 'rxjs';
import { AuthStore } from './auth.store';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/Auth`;
  private authStore = inject(AuthStore);
  private http = inject(HttpClient);

  login(request: LoginRequest) {
    return this.http.post<string>(`${this.apiUrl}/login`, request, {
      responseType: 'text',
      withCredentials: true,
    });
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

  initialize(): Observable<CurrentUser | null> {
    return this.currentUser().pipe(
      tap((user) => this.authStore.setUser(user)),
      catchError(() => {
        this.authStore.clear();
        return of(null);
      }),
    );
  }
}
