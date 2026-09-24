import { Routes } from '@angular/router';
import { CreatePersonComponent } from './create-person/create-person.component';
import { PeopleSummaryComponent } from './people-summary/people-summary.component';
import { PersonDetailsComponent } from './person-details/person-details.component';
import { EditPersonComponent } from './edit-person/edit-person.component';

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
    path: ':personId/edit',
    component: EditPersonComponent,
  },
  {
    path: ':personId',
    component: PersonDetailsComponent,
  },
];
