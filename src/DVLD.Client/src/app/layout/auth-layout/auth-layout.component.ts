import { Component } from '@angular/core';
import { LoginComponent } from '../../features/auth/pages/auth/login/login.component';

@Component({
  imports: [LoginComponent],
  selector: 'app-auth-layout',
  styleUrl: './auth-layout.component.css',
  templateUrl: './auth-layout.component.html',
})
export class AuthLayoutComponent {}
