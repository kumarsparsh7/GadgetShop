import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, inject, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-customer-dialog-box',
  imports: [FormsModule, CommonModule],
  templateUrl: './customer-dialog-box.html',
  styleUrl: './customer-dialog-box.css'
})
export class CustomerDialogBox {

  @Input() private customer: any;

  modal = inject(NgbActiveModal);
  customerDetails = {
    customerId: "",
    firstName: "",
    lastName: "",
    registrationDate: "",
    phoneNo: "",
    email: ""
  }
  httpClient = inject(HttpClient);

  btnText: string = "Add";
  disableCustomerIdInput: boolean = false;
  ngOnInit() {
    if(this.customer != null) {
      this.customerDetails = this.customer;
      this.btnText = "Update";
      this.disableCustomerIdInput = true;
    }
  }

  onSubmit() {
    let apiUrl = environment.apiUrl + "Customer";
    let httpOptions = {
      headers: new HttpHeaders({
        Authorization: 'my-auth-token',
        'Content-Type': "application/json"
      })
    }
    if(this.btnText != "Update") {
      this.httpClient.post(apiUrl, this.customerDetails, httpOptions).subscribe({
        next: v => console.log(v),
        error: e => console.log(e),
        complete: () => {
          alert("Customer details saved successfully" + JSON.stringify(this.customerDetails));
          this.modal.close({event:"closed"}); // To close modal popup after submission
        }
      })
    } else {
      this.httpClient.put(apiUrl, this.customerDetails, httpOptions).subscribe({
        next: v => console.log(v),
        error: e => console.log(e),
        complete: () => {
          alert("Customer details updated successfully" + JSON.stringify(this.customerDetails));
          this.modal.close({event:"closed"}); // To close modal popup after submission
        }
      })
    }
  }
}
