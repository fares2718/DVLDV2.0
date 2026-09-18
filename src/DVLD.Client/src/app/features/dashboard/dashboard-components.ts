import { Type } from '@angular/core';
import { Permission } from '../../core/auth/permissions';
import { PeopleComponent } from './components/people/people.component';

export interface DashboardComponent {
  permission: Permission;
  component: Type<unknown>;
}

export const DashboardComponents: DashboardComponent[] = [
  {
    permission: Permission.ViewPeople,
    component: PeopleComponent,
  },
];
