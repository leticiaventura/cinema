import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HistoryComponent } from './history.component';
import { CustomerAuthGuard } from 'src/app/shared/guards/customer-auth.guard';

const routes: Routes = [{ path: '', component: HistoryComponent, canActivate: [CustomerAuthGuard] }];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class HistoryRoutingModule { }
