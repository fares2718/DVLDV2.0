import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { PeopleService } from '../../../../core/people/people.service';

@Component({
  imports: [RouterLink],
  selector: 'app-people-view',
  styleUrl: './people.component.css',
  templateUrl: './people.component.html',
})
export class PeopleComponent implements OnInit {
  private peopleService = inject(PeopleService);
  peopleCount = signal<number>(this.peopleService.count);
  ngOnInit(): void {
    this.peopleService.getPeopleCount().subscribe((count) => {
      this.peopleCount.set(count);
    });
  }
}
