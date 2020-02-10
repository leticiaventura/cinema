import { MaterialModule } from './../../shared/material/material.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InTheatersRoutingModule } from './in-theaters-routing.module';
import { InTheatersComponent } from './in-theaters.component';
import { CustomerAuthGuard } from 'src/app/shared/guards/customer-auth.guard';


@NgModule({
  declarations: [InTheatersComponent],
  imports: [
    CommonModule,
    InTheatersRoutingModule,
    MaterialModule
  ],
  providers: [CustomerAuthGuard]
})
export class InTheatersModule { }
