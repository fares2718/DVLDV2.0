import { Component, input, signal } from '@angular/core';
import { Address } from '../models/address';

@Component({
  selector: 'app-address',
  standalone: true,
  templateUrl: './address.component.html',
})
export class AddressComponent {
  readonly addresses = input<Address[]>([]);

  readonly selectedAddressId = signal<string | null>(null);
  protected readonly pendingAddressAction = signal<
    'activate' | 'deactivate' | 'primary' | 'not-primary' | null
  >(null);

  protected selectAddress(addressId: string): void {
    this.selectedAddressId.set(addressId);
  }

  protected requestAddressAction(
    action: 'activate' | 'deactivate' | 'primary' | 'not-primary',
  ): void {
    this.pendingAddressAction.set(action);
  }

  protected closeAddressActionDialog(): void {
    this.pendingAddressAction.set(null);
  }

  protected get selectedAddress(): Address | null {
    const addresses = this.addresses();

    if (addresses.length === 0) {
      return null;
    }

    const selectedId = this.selectedAddressId();

    if (selectedId) {
      return addresses.find((a) => a.addressId === selectedId) ?? addresses[0];
    }

    return addresses.find((a) => a.isPrimary) ?? addresses[0];
  }

  protected addressLabel(address: Address): string {
    return address.addressType || 'Address';
  }

  protected formatAddress(address: Address): string {
    const parts = [
      address.street,
      address.buildingNumber ? `Building ${address.buildingNumber}` : null,
      address.apartmentNumber ? `Apartment ${address.apartmentNumber}` : null,
      address.city,
      address.governorate,
      address.countryCode,
    ];

    return parts.filter(Boolean).join(', ');
  }
}
