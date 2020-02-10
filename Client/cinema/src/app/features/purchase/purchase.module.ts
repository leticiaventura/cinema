import { SnackService } from './../snacks/snack.service';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CustomerAuthGuard } from 'src/app/shared/guards/customer-auth.guard';
import { MaterialModule } from './../../shared/material/material.module';
import { PurchaseResolveService } from './purchase.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PurchaseRoutingModule } from './purchase-routing.module';
import { PurchaseComponent } from './purchase.component';


@NgModule({
    declarations: [PurchaseComponent],
    imports: [
        CommonModule,
        PurchaseRoutingModule,
        MaterialModule,
        ReactiveFormsModule,
        FormsModule,
    ],
    providers: [
        PurchaseResolveService, CustomerAuthGuard, SnackService
    ]
})
export class PurchaseModule { }
