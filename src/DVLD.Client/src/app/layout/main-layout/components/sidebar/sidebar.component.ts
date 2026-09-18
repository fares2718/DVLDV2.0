import { Component, computed, inject } from '@angular/core';
import { Permission } from '../../../../core/auth/permissions';
import { AuthStore } from '../../../../core/auth/auth.store';
import { RouterLink, RouterLinkActive } from '@angular/router';

interface SidebarItem {
  label: string;
  route: string;
  permission: Permission;
}

@Component({
  imports: [RouterLink, RouterLinkActive],
  selector: 'app-sidebar',
  styleUrl: './sidebar.component.css',
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent {
  private readonly authStore = inject(AuthStore);

  private readonly items: SidebarItem[] = [
    {
      label: 'Dashboard',
      route: '/dashboard',
      permission: Permission.None,
    },
    {
      label: 'People',
      route: '/people',
      permission: Permission.ViewPeople,
    },
    {
      label: 'Applications',
      route: '/applications',
      permission: Permission.ViewApplications,
    },
    {
      label: 'Licenses',
      route: '/licenses',
      permission: Permission.ViewLicenses,
    },
    {
      label: 'Tests',
      route: '/tests',
      permission: Permission.ViewTests,
    },
    {
      label: 'Users',
      route: '/users',
      permission: Permission.ViewUsers,
    },
  ];

  readonly visibleItems = computed(() =>
    this.items.filter(
      (item) =>
        item.permission === Permission.None || this.authStore.hasPermission(item.permission),
    ),
  );
}
