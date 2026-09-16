import { Routes } from '@angular/router';

export const authRoutes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login.component').then((m) => m.LoginComponent),
  },

  {
    path: 'unauthorized',
    loadComponent: () =>
      import('./pages/unauthorized/unauthorized.component').then((m) => m.UnauthorizedComponent),
  },
];
