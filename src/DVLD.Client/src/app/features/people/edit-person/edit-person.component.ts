import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { PeopleService } from '../../../core/people/people.service';
import { environment } from '../../../../environments/environment.development';
import type {
  EditPersonContactInfo,
  EditPersonName,
  EditPersonalInfo,
} from '../models/edit-person';

@Component({
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  selector: 'app-edit-person',
  styleUrl: './edit-person.component.css',
  templateUrl: './edit-person.component.html',
})
export class EditPersonComponent implements OnInit {
  private readonly _fb = inject(FormBuilder);
  private readonly _route = inject(ActivatedRoute);
  private readonly _peopleService = inject(PeopleService);

  readonly nameForm = this._fb.nonNullable.group({
    firstName: ['', Validators.required],
    secondName: ['', Validators.required],
    thirdName: [''],
    lastName: ['', Validators.required],
    motherName: ['', Validators.required],
  });
  readonly contactForm = this._fb.nonNullable.group({
    phone: ['', Validators.required],
    altPhone: [''],
  });
  readonly personalForm = this._fb.nonNullable.group({
    dateOfBirth: ['', Validators.required],
    nationalityCountryCode: ['', Validators.required],
  });

  personId = '';
  selectedImage: File | null = null;
  imagePreviewUrl = '';
  currentImageUrl = '';
  message = '';
  errorMessage = '';

  ngOnInit(): void {
    this.personId = this._route.snapshot.paramMap.get('personId') ?? '';
    if (!this.personId) return;

    this._peopleService.getPersonById(this.personId).subscribe({
      next: (person) => {
        const nameParts = person.fullName.trim().split(/\s+/).filter(Boolean);
        this.nameForm.patchValue({
          firstName: person.firstName ?? nameParts[0] ?? '',
          secondName: person.secondName ?? nameParts[1] ?? '',
          thirdName: person.thirdName ?? nameParts[2] ?? '',
          lastName: person.lastName ?? nameParts.at(-1) ?? '',
          motherName: person.motherName ?? '',
        });
        this.contactForm.patchValue({ phone: person.phone ?? '', altPhone: person.altPhone ?? '' });
        this.personalForm.patchValue({
          dateOfBirth: person.dateOfBirth
            ? new Date(person.dateOfBirth).toISOString().slice(0, 10)
            : '',
          nationalityCountryCode: person.nationality ?? '',
        });
        this.currentImageUrl = this.personImageUrl(person.imagePath);
      },
      error: () => (this.errorMessage = 'Unable to load this person.'),
    });
  }

  submitName(): void {
    if (this.nameForm.invalid) return this.nameForm.markAllAsTouched();
    this.submit(() =>
      this._peopleService.updatePersonName(
        this.personId,
        this.nameForm.getRawValue() as EditPersonName,
      ),
    );
  }

  submitContact(): void {
    if (this.contactForm.invalid) return this.contactForm.markAllAsTouched();
    this.submit(() =>
      this._peopleService.updatePersonContactInfo(
        this.personId,
        this.contactForm.getRawValue() as EditPersonContactInfo,
      ),
    );
  }

  submitPersonal(): void {
    if (this.personalForm.invalid) return this.personalForm.markAllAsTouched();
    this.submit(() =>
      this._peopleService.updatePersonalInfo(
        this.personId,
        this.personalForm.getRawValue() as EditPersonalInfo,
      ),
    );
  }

  chooseImage(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedImage = input.files?.[0] ?? null;
    if (this.selectedImage) {
      this.imagePreviewUrl = URL.createObjectURL(this.selectedImage);
    }
  }

  uploadImage(): void {
    if (!this.selectedImage) return;
    this.clearFeedback();
    const name = this.nameForm.getRawValue();
    const personName = [name.firstName, name.secondName, name.thirdName, name.lastName]
      .filter(Boolean)
      .join(' ');

    this._peopleService.uploadPersonImage(this.personId, this.selectedImage, personName).subscribe({
      next: () => {
        this.message = 'Profile image updated successfully.';
        this.currentImageUrl = this.imagePreviewUrl;
        this.selectedImage = null;
      },
      error: () => (this.errorMessage = 'Unable to update the profile image.'),
    });
  }

  private submit(request: () => ReturnType<PeopleService['updatePersonName']>): void {
    this.clearFeedback();
    request().subscribe({
      next: () => (this.message = 'Changes saved successfully.'),
      error: () => (this.errorMessage = 'Unable to save these changes.'),
    });
  }

  private clearFeedback(): void {
    this.message = '';
    this.errorMessage = '';
  }

  private personImageUrl(imagePath: string | null | undefined): string {
    const rawImageUrl = imagePath?.trim();
    if (!rawImageUrl || rawImageUrl === 'string' || rawImageUrl === 'null') return '';
    if (/^https?:\/\//i.test(rawImageUrl)) return rawImageUrl;

    const serverBaseUrl = environment.apiUrl.replace(/\/$/, '').replace(/\/api$/i, '');
    const normalizedPath = rawImageUrl.replace(/\\/g, '/');
    const pathWithoutWwwRoot = normalizedPath.includes('/wwwroot/')
      ? normalizedPath.split(/\/wwwroot\//i)[1]
      : normalizedPath
          .replace(/^.*?\/wwwroot\//i, '')
          .replace(/^wwwroot\//i, '')
          .replace(/^\//, '');

    return pathWithoutWwwRoot
      ? new URL(`/${pathWithoutWwwRoot}`, `${serverBaseUrl}/`).toString()
      : '';
  }
}
