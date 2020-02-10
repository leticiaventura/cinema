import { PurchaseResolveService } from './purchase.service';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PurchaseComponent } from './purchase.component';
import { CustomerAuthGuard } from 'src/app/shared/guards/customer-auth.guard';

const routes: Routes = [
    {
        path: ':id',
        component: PurchaseComponent,
        resolve: {
            movie: PurchaseResolveService
        },
        canActivate: [CustomerAuthGuard] 
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PurchaseRoutingModule { }
