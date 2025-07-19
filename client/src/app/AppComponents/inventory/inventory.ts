import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogBox } from '../dialog-box/dialog-box';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-inventory',
  imports: [FormsModule, CommonModule, DialogBox],
  templateUrl: './inventory.html',
  styleUrl: './inventory.css'
})

export class Inventory {
  constructor(private cdr: ChangeDetectorRef){}
  private modalService = inject(NgbModal);
  httpClient = inject(HttpClient);
  inventoryData = {
    productId: "",
    productName: "",
    availableQty: 0,
    reorderPoint: 0
  }

  // GET Step 1: To make GET request to backend whenever component is initialized
  inventoryDetails: any;
  ngOnInit() {
    this.getInventoryDetails();
  }
  
  getInventoryDetails() {
    let apiUrl = environment.apiUrl + "Inventory";
    this.httpClient.get(apiUrl).subscribe(
      data => {
        this.inventoryDetails = data;
        console.log(this.inventoryDetails);
        this.cdr.detectChanges();
      }
    );
    this.inventoryData = {
    productId: "",
    productName: "",
    availableQty: 0,
    reorderPoint: 0
  };
  this.disableProductIdInput = false;
  }

  // POST Step 1: To make POST request to server with details from form component
  onSubmit(): void {
    let apiUrl = environment.apiUrl + "Inventory";
    let httpOptions = {
      headers: new HttpHeaders(
        {
          Authorization:'my-auth-token',
          'Content-Type':"application/json"
        }
      )
    }
    if(this.disableProductIdInput) {
      this.httpClient.put(apiUrl, this.inventoryData, httpOptions).subscribe({
      next: v => console.log(v),
      error: e => console.log(e),
      complete: () => {
        alert("Data updated successfully" + "\n" + JSON.stringify(this.inventoryData));
        this.getInventoryDetails();
        this.cdr.detectChanges();
      }
    });
    } else {
      this.httpClient.post(apiUrl, this.inventoryData, httpOptions).subscribe({
        next: v => console.log(v),
        error: e => console.log(e),
        complete: () => {
          alert("Form submitted successfully" + "\n" + JSON.stringify(this.inventoryData));
          this.getInventoryDetails();
          this.cdr.detectChanges();
        }
      });
    }
  }

  // DELETE
  productIdToDelete: number = 0;
  openConfirmDialog(productId: number) {
    // Open dialog-box component when this function is called
    this.productIdToDelete = productId;
    this.modalService.open(DialogBox).result.then(data => {
      if(data.event === "confirm") {
        console.log("Confirmed to delete...");
        this.deleteInventory();
      } else {
        console.log("Delete not required");
      }
    });
  }
  deleteInventory(){
    let apiUrl = environment.apiUrl + "Inventory?ProductId="+this.productIdToDelete;
    this.httpClient.delete(apiUrl).subscribe();
    this.getInventoryDetails();
    this.cdr.detectChanges();
  }

  // UPDATE
  disableProductIdInput = false;
  populateFormForEdit(inventory: any) {
    this.disableProductIdInput = true;
    this.inventoryData.productId = inventory.ProductId;
    this.inventoryData.productName = inventory.ProductName;
    this.inventoryData.availableQty = inventory.AvailableQty;
    this.inventoryData.reorderPoint = inventory.ReorderPoint;
  }
}
