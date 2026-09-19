import { Routes } from '@angular/router';
import { CreatePersonComponent } from './create-person/create-person.component';
import { PeopleSummaryComponent } from './people-summary/people-summary.component';
import { PersonDetailsComponent } from './person-details/person-details.component';

export const peopleRoutes: Routes = [
  {
    path: '',
    component: PeopleSummaryComponent,
  },
  {
    path: 'create',
    component: CreatePersonComponent,
  },
  {
    path: ':personId',
    component: PersonDetailsComponent,
  },
];
