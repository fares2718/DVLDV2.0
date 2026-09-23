import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { environment } from '../../../../environments/environment.development';
import type { PersonSummary } from '../models/person-summary';
import { Address } from '../models/address';
import { AddressService } from '../../../core/people/address.service';
import { AddressComponent } from '../address/address.component';
import { PeopleService } from '../../../core/people/people.service';
import { switchMap } from 'rxjs';
import { AuthStore } from '../../../core/auth/auth.store';
import { Permission } from '../../../core/auth/permissions';

type StatusNotification = {
  type: 'success' | 'error';
  title: string;
  message: string;
};

@Component({
  selector: 'app-person-details',
  standalone: true,
  imports: [RouterLink, AddressComponent],
  styleUrl: './person-details.component.css',
  templateUrl: './person-details.component.html',
})
export class PersonDetailsComponent implements OnInit {
  private readonly _route = inject(ActivatedRoute);
  private addressService = inject(AddressService);
  private personService = inject(PeopleService);
  private statusNotificationTimer?: ReturnType<typeof setTimeout>;
  readonly authStore = inject(AuthStore);

  permission = Permission;

  addresses = signal<Address[]>([]);
  protected readonly pendingPersonAction = signal<'activate' | 'deactivate' | null>(null);
  protected readonly statusNotification = signal<StatusNotification | null>(null);

  person = signal<PersonSummary | null>(null);

  personImageUrl(person: PersonSummary | null): string | null {
    if (!person) {
      return null;
    }

    const rawImageUrl = person.imagePath?.trim();

    if (!rawImageUrl || rawImageUrl === 'string' || rawImageUrl === 'null') {
      return null;
    }

    if (/^https?:\/\//i.test(rawImageUrl)) {
      return rawImageUrl;
    }

    const serverBaseUrl = environment.apiUrl.replace(/\/$/, '').replace(/\/api$/i, '');
    const normalizedPath = rawImageUrl.replace(/\\/g, '/');
    const pathWithoutWwwRoot = normalizedPath.includes('/wwwroot/')
      ? normalizedPath.split(/\/wwwroot\//i)[1]
      : normalizedPath
          .replace(/^.*?\/wwwroot\//i, '')
          .replace(/^wwwroot\//i, '')
          .replace(/^\//, '');

    if (!pathWithoutWwwRoot || pathWithoutWwwRoot === 'string') {
      return null;
    }

    return new URL(`/${pathWithoutWwwRoot}`, `${serverBaseUrl}/`).toString();
  }

  personInitial(person: PersonSummary | null): string {
    if (!person) {
      return '?';
    }

    return person.fullName?.trim()?.charAt(0)?.toUpperCase() ?? '?';
  }

  protected requestPersonAction(action: 'activate' | 'deactivate'): void {
    this.closeStatusNotification();
    this.pendingPersonAction.set(action);
  }

  protected closePersonActionDialog(): void {
    this.pendingPersonAction.set(null);
  }

  protected changePersonStatus(): void {
    const personId = this.person()?.personId;
    const action = this.pendingPersonAction();

    if (!personId || !action) {
      return;
    }

    this.closeStatusNotification();

    const statusChange$ =
      action === 'activate'
        ? this.personService.activatePerson(personId)
        : this.personService.deactivatePerson(personId);

    statusChange$.pipe(switchMap(() => this.personService.getPersonById(personId))).subscribe({
      next: (updatedPerson) => {
        this.person.set(updatedPerson);
        this.closePersonActionDialog();
        this.showStatusNotification({
          type: 'success',
          title: 'Status updated',
          message:
            action === 'activate'
              ? 'Person activated successfully.'
              : 'Person deactivated successfully.',
        });
      },
      error: (error) => {
        console.error('Error changing person status:', error);
        this.closePersonActionDialog();
        this.showStatusNotification({
          type: 'error',
          title: 'Status update failed',
          message: error?.error?.message ?? error?.message ?? 'Unable to change the person status.',
        });
      },
    });
  }

  private showStatusNotification(notification: StatusNotification): void {
    this.closeStatusNotification();
    this.statusNotification.set(notification);
    this.statusNotificationTimer = setTimeout(() => {
      this.statusNotification.set(null);
      this.statusNotificationTimer = undefined;
    }, 3000);
  }

  protected closeStatusNotification(): void {
    if (this.statusNotificationTimer) {
      clearTimeout(this.statusNotificationTimer);
      this.statusNotificationTimer = undefined;
    }

    this.statusNotification.set(null);
  }

  ngOnInit(): void {
    this._route.paramMap.subscribe((params) => {
      const personId = params.get('personId');
      if (!personId) {
        return;
      }

      this.personService.getPersonById(personId).subscribe({
        next: (person) => {
          this.person.set(person);
        },
        error: (error) => {
          console.error('Error fetching person details:', error);
        },
      });

      this.addressService.getPersonAddresses(personId).subscribe({
        next: (addresses) => {
          this.addresses.set(addresses);
        },
        error: (error) => {
          console.error('Error fetching person addresses:', error);
        },
      });
    });
  }
}
