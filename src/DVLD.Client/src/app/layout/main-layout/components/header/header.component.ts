import { Component, inject } from '@angular/core';
import { AuthStore } from '../../../../core/auth/auth.store';

@Component({
  imports: [],
  selector: 'app-header',
  styleUrl: './header.component.css',
  templateUrl: './header.component.html',
})
export class HeaderComponent {
  readonly authStore = inject(AuthStore);
}
