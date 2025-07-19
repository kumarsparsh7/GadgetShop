import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomerDialogBox } from '../customer-dialog-box/customer-dialog-box';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { DialogBox } from '../dialog-box/dialog-box';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './customer.html',
  styleUrl: './customer.css'
})
export class Customer {
  constructor(private cdr: ChangeDetectorRef) {}
  // Component opened on click
  private modalService = inject(NgbModal)
  openCustomerDialog() {
    this.modalService.open(CustomerDialogBox).result.then(
      data => {
        if(data.event == "closed") {
          this.getCustomerDetails();
        }
      }
    );
  }

  httpClient = inject(HttpClient);
  ngOnInit() {
    this.getCustomerDetails();
  }

  customerDetails: any;
  getCustomerDetails() {
    let apiUrl = environment.apiUrl + "Customer";
    this.httpClient.get(apiUrl).subscribe(result => {
      this.customerDetails = result;
      this.cdr.detectChanges();
    });
  }

  openConfirmDialog(customerId: number) {
    this.modalService.open(DialogBox).result.then(data => {
      if(data.event === "confirm") {
        console.log("Confirmed to delete...");
        this.deleteCustomer(customerId);
        this.getCustomerDetails();
      } else {
        console.log("Delete not required");
      }
    });
  }
  deleteCustomer(customerId: number) {
    let apiUrl = environment.apiUrl + "Customer?CustomerId=" + customerId;
    this.httpClient.delete(apiUrl).subscribe({
      next: () => {
        this.getCustomerDetails();
      }
    });
  }

  openEditDialogBox(customer: any) {
    const modalReference = this.modalService.open(CustomerDialogBox);
    modalReference.componentInstance.customer = {
      customerId: customer.CustomerId,
      firstName: customer.FirstName,
      lastName: customer.LastName,
      email: customer.Email,
      registrationDate: customer.RegistrationDate,
      phoneno: customer.PhoneNo
    };
    modalReference.result.then(
      data => {
        if(data.event == "closed") {
          this.getCustomerDetails();
        }
      }
    )
  }
}
