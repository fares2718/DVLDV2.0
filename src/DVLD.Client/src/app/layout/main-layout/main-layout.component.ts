import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';

@Component({
  imports: [RouterOutlet, HeaderComponent, SidebarComponent],
  selector: 'app-main-layout',
  styleUrl: './main-layout.component.css',
  templateUrl: './main-layout.component.html',
})
export class MainLayoutComponent {
  readonly mobileSidebarOpen = signal(false);
  readonly sidebarCollapsed = signal(false);

  protected toggleSidebar(): void {
    if (window.matchMedia('(max-width: 1023px)').matches) {
      this.mobileSidebarOpen.update((open) => !open);
      return;
    }

    this.sidebarCollapsed.update((collapsed) => !collapsed);
  }

  protected closeSidebar(): void {
    this.mobileSidebarOpen.set(false);
  }
}
