import { AccountsRoutingModule } from './account-routing.module';
import { AccountComponent } from './account.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../shared/material/material.module';


@NgModule({
    declarations: [AccountComponent],
    imports: [
        CommonModule,
        AccountsRoutingModule,
        MaterialModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule,
        MaterialModule
    ],
    providers: []
})
export class AccountsModule { }
