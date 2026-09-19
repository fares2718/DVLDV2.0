import { Component, computed, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PeopleService } from '../../../core/people/people.service';
import type { PeopleFilter } from '../models/people-filter';

@Component({
  selector: 'app-people-summary',
  standalone: true,
  imports: [FormsModule],
  styleUrl: './people-summary.component.css',
  templateUrl: './people-summary.component.html',
})
export class PeopleSummaryComponent implements OnInit {
  private readonly _peopleService = inject(PeopleService);

  private readonly defaultFilter: PeopleFilter = {
    isDescending: false,
    pageNumber: 1,
    pageSize: 10,
  };

  filter: PeopleFilter = { ...this.defaultFilter };

  readonly people = computed(() => this._peopleService.peopleSummary()?.items ?? []);
  readonly totalCount = computed(() => this._peopleService.peopleSummary()?.totalCount ?? 0);
  readonly pageNumber = computed(
    () => this._peopleService.peopleSummary()?.pageNumber ?? this.filter.pageNumber ?? 1,
  );
  readonly pageSize = computed(
    () => this._peopleService.peopleSummary()?.pageSize ?? this.filter.pageSize ?? 10,
  );
  readonly totalPages = computed(() => {
    const total = this.totalCount();
    const size = this.pageSize() || 1;

    return total === 0 ? 1 : Math.ceil(total / size);
  });
  readonly hasNextPage = computed(() => this.pageNumber() < this.totalPages());
  readonly hasPreviousPage = computed(() => this.pageNumber() > 1);

  ngOnInit(): void {
    this.loadPeople();
  }

  private normalize(value: string | null | undefined): string | null | undefined {
    return (value ?? '').trim() || null;
  }

  loadPeople(): void {
    const request: PeopleFilter = {
      ...this.filter,
      search: this.normalize(this.filter.search),
      nationalId: this.normalize(this.filter.nationalId),
      name: this.normalize(this.filter.name),
      gender: this.normalize(this.filter.gender),
      phone: this.normalize(this.filter.phone),
      email: this.normalize(this.filter.email),
      sortBy: this.normalize(this.filter.sortBy),
      isActive:
        this.filter.isActive === undefined || this.filter.isActive === null
          ? undefined
          : this.filter.isActive,
      pageNumber: this.filter.pageNumber ?? 1,
      pageSize: this.filter.pageSize ?? 10,
      isDescending: this.filter.isDescending ?? false,
    };

    this._peopleService.getPeopleSummary(request).subscribe();
  }

  applyFilters(): void {
    this.filter.pageNumber = 1;
    this.loadPeople();
  }

  resetFilters(): void {
    this.filter = { ...this.defaultFilter };
    this.loadPeople();
  }

  previousPage(): void {
    if (!this.hasPreviousPage()) {
      return;
    }

    this.filter.pageNumber = (this.filter.pageNumber ?? 1) - 1;
    this.loadPeople();
  }

  nextPage(): void {
    if (!this.hasNextPage()) {
      return;
    }

    this.filter.pageNumber = (this.filter.pageNumber ?? 1) + 1;
    this.loadPeople();
  }

  onPageSizeChange(value: string): void {
    const pageSize = Number(value);

    if (!Number.isFinite(pageSize) || pageSize <= 0) {
      return;
    }

    this.filter.pageSize = pageSize;
    this.filter.pageNumber = 1;
    this.loadPeople();
  }
}
