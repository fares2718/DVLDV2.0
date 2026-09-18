import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, EMPTY, switchMap, throwError } from 'rxjs';

import { AuthService } from './auth.service';
import { AuthStore } from './auth.store';

export const refreshInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const authStore = inject(AuthStore);

  return next(req).pipe(
    catchError((error) => {
      if (
        error.status === 401 &&
        !req.url.includes('/auth/refresh') &&
        !req.url.includes('/auth/current-user') &&
        error.error?.message === 'Token is expired'
      ) {
        return authService.refresh().pipe(
          //refresh current user
          switchMap(() => authService.currentUser()),

          //CurrentUser
          switchMap((user) => {
            authStore.setUser(user);
            //resend request
            return next(req);
          }),

          catchError((refreshError) => {
            authStore.clear();
            authService.logout().pipe(
              catchError(() => EMPTY),
              switchMap(() => throwError(() => refreshError)),
            );

            return throwError(() => refreshError);
          }),
        );
      }

      return throwError(() => error);
    }),
  );
};
