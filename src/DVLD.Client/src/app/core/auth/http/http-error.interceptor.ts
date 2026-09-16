import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const httpErrorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      switch (error.status) {
        case 403:
          // Authenticated but not authorized
          // Navigate to /auth/unauthorized
          break;
      }

      return throwError(() => error);
    }),
  );
};
