import { Component, signal } from '@angular/core';
import { RouterOutlet, RouterModule } from '@angular/router';
import { Inventory } from './AppComponents/inventory/inventory';
import { Customer } from './AppComponents/customer/customer';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet, 
    RouterModule, 
    Inventory,  // Adding created component in main app
    Customer
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('client');
}
