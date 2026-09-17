import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { inject } from '@angular/core';

export const refreshInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  return next(req).pipe(
    catchError((error) => {
      if (
        error.status === 401 &&
        !req.url.includes('/auth/refresh') &&
        error.error?.message === 'Token expired'
      ) {
        // Handle token refresh logic here
        // For example, you can call a refresh token API and retry the request
      }
      return throwError(() => error);
    }),
  );
};
