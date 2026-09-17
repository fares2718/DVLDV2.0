import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../../../core/auth/auth.service';
import { LoginRequest } from '../../../models/login-request';
import { Router } from '@angular/router';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-login',
  styleUrl: './login.component.css',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  private authService = inject(AuthService);
  private route = inject(Router);
  form = new FormGroup({
    username: new FormControl('', {
      validators: [Validators.required],
      nonNullable: true,
    }),

    password: new FormControl('', {
      validators: [Validators.required, Validators.minLength(8), Validators.maxLength(100)],
      nonNullable: true,
    }),
  });

  onSubmit(): void {
    console.log('SUBMIT FIRED');

    console.log('Form value:', this.form.getRawValue());
    console.log('Form valid:', this.form.valid);
    console.log('Form errors:', this.form.errors);

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    console.log('LOGIN DATA:', this.form.getRawValue());
    if (this.form.invalid) {
      return;
    }
    const loginRequest: LoginRequest = {
      username: this.form.get('username')!.value,
      password: this.form.get('password')!.value,
    };
    this.authService.login(loginRequest).subscribe({
      next: (response) => {
        console.log(response);
      },

      error: (error) => {
        console.error('LOGIN ERROR:', error);
      },

      complete: () => {
        console.log('LOGIN COMPLETED');
        this.authService.currentUser().subscribe({
          next: (currentUser) => {
            console.log('CURRENT USER:', currentUser);
          },
        });
      },
    });
  }
}
