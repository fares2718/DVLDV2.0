import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { environment } from '../../../../environments/environment.development';
import type { PersonSummary } from '../models/person-summary';
import { Address } from '../models/address';
import { AddressService } from '../../../core/people/address.service';
import { AddressComponent } from '../address/address.component';

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

  addresses = signal<Address[]>([]);
  protected readonly pendingPersonAction = signal<'activate' | 'deactivate' | null>(null);

  readonly person = computed<PersonSummary | null>(() => {
    const state = history.state as { person?: PersonSummary } | undefined;
    return state?.person ?? null;
  });

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
    this.pendingPersonAction.set(action);
  }

  protected closePersonActionDialog(): void {
    this.pendingPersonAction.set(null);
  }

  ngOnInit(): void {
    this._route.paramMap.subscribe((params) => {
      const personId = params.get('personId');
      if (!personId) {
        return;
      }

      const state = history.state as { person?: PersonSummary } | undefined;
      if (!state?.person) {
        console.warn(`Person details requested for ${personId} without state payload.`);
      }

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
