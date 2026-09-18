import { Component, computed, inject } from '@angular/core';
import { AuthStore } from '../../core/auth/auth.store';
import { DashboardComponents } from './dashboard-components';
import { NgComponentOutlet } from '@angular/common';

@Component({
  imports: [NgComponentOutlet],
  selector: 'app-dashboard',
  styleUrl: './dashboard.component.css',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent {
  private readonly authStore = inject(AuthStore);

  readonly components = computed(() =>
    DashboardComponents.filter((cmp) => this.authStore.hasPermission(cmp.permission)),
  );
}
