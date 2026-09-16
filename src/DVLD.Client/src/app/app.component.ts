import { Component, signal } from '@angular/core';
import { AuthLayoutComponent } from './core/features/layout/auth-layout/auth-layout.component';

@Component({
  selector: 'app-root',
  imports: [AuthLayoutComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class App {
  protected readonly title = signal('DVLD.Client');
}
