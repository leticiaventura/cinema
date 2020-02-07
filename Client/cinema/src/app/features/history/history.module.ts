import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HistoryRoutingModule } from './history-routing.module';
import { HistoryComponent } from './history.component';
import { CustomerAuthGuard } from 'src/app/shared/guards/customer-auth.guard';


@NgModule({
  declarations: [HistoryComponent],
  imports: [
    CommonModule,
    HistoryRoutingModule
  ],
  providers: [CustomerAuthGuard]
})
export class HistoryModule { }
