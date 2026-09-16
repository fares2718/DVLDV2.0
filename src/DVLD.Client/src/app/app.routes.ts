import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./core/features/auth/auth.routs').then((m) => m.authRoutes),
  },
];
