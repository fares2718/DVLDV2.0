import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../../../core/auth/auth.service';
import { LoginRequest } from '../../../models/login-request';
import { Router } from '@angular/router';
import { AuthStore } from '../../../../../core/auth/auth.store';
import { switchMap } from 'rxjs';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-login',
  styleUrl: './login.component.css',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  private authService = inject(AuthService);
  private authStore = inject(AuthStore);
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

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const loginRequest: LoginRequest = {
      username: this.form.get('username')!.value,
      password: this.form.get('password')!.value,
    };

    this.authService
      .login(loginRequest)
      .pipe(switchMap(() => this.authService.currentUser()))
      .subscribe({
        next: (currentUser) => {
          console.log('CURRENT USER:', currentUser);

          this.authStore.setUser(currentUser);

          this.route.navigate(['/dashboard']);
        },

        error: (error) => {
          console.error('LOGIN ERROR:', error);
        },
      });
  }
}
