import { Type } from '@angular/core';
import { Permission } from '../../core/auth/permissions';

export interface DashboardComponent {
  permission: Permission;
  component: Type<unknown>;
}

export const DashboardComponents: DashboardComponent[] = [];
