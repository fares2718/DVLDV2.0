import { computed, Injectable, signal } from '@angular/core';
import { CurrentUser } from '../../features/auth/models/current-user';
import { Permission } from './permissions';

@Injectable({
  providedIn: 'root',
})
export class AuthStore {
  private readonly _user = signal<CurrentUser | null>(null);

  private readonly _isInitialized = signal(false);

  readonly user = this._user.asReadonly();

  readonly isInitialized = this._isInitialized.asReadonly();

  readonly isAuthenticated = computed(() => this._user() !== null);

  setUser(user: CurrentUser) {
    this._user.set(user);
  }

  clear() {
    this._user.set(null);
  }

  setInitialized() {
    this._isInitialized.set(true);
  }

  hasPermission(permission: Permission): boolean {
    const user = this.user();

    if (!user) {
      return false;
    }

    return (user.permissions & permission) === permission;
  }
}
