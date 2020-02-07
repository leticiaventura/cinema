import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { InTheatersComponent } from './in-theaters.component';
import { CustomerAuthGuard } from 'src/app/shared/guards/customer-auth.guard';

const routes: Routes = [{ path: '', component: InTheatersComponent, canActivate: [CustomerAuthGuard] }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InTheatersRoutingModule { }
