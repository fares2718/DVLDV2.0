import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { PeopleService } from '../../../core/people/people.service';
import type { CreatePerson } from '../models/create-person';

@Component({
  selector: 'app-create-person',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  styleUrl: './create-person.component.css',
  templateUrl: './create-person.component.html',
})
export class CreatePersonComponent {
  private readonly _fb = inject(FormBuilder);
  private readonly _peopleService = inject(PeopleService);
  private readonly _router = inject(Router);

  readonly form = this._fb.group({
    nationalId: ['', Validators.required],
    firstName: ['', Validators.required],
    secondName: ['', Validators.required],
    thirdName: [''],
    lastName: ['', Validators.required],
    motherName: ['', Validators.required],
    dateOfBirth: ['', Validators.required],
    phone: ['', Validators.required],
    gender: [true, Validators.required],
    email: ['', [Validators.required, Validators.email]],
    nationalityCountryCode: ['', Validators.required],
    altPhone: [''],
  });

  submitPerson(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();
    const person: CreatePerson = {
      nationalId: value.nationalId?.trim() ?? '',
      firstName: value.firstName?.trim() ?? '',
      secondName: value.secondName?.trim() ?? '',
      thirdName: value.thirdName?.trim() ? value.thirdName.trim() : null,
      lastName: value.lastName?.trim() ?? '',
      motherName: value.motherName?.trim() ?? '',
      dateOfBirth: value.dateOfBirth ?? '',
      phone: value.phone?.trim() ?? '',
      gender: value.gender === true,
      email: value.email?.trim() ?? '',
      nationalityCountryCode: value.nationalityCountryCode?.trim() ?? '',
      altPhone: value.altPhone?.trim() ? value.altPhone.trim() : null,
    };

    this._peopleService.createPerson(person).subscribe({
      next: () => this._router.navigate(['/people']),
      error: () => console.error('Failed to create person.'),
    });
  }
}
