import { EmployeeAuthGuard } from './../../shared/guards/employee-auth.guard';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CheckInComponent } from './check-in.component';

const routes: Routes = [{ path: '', component: CheckInComponent, canActivate: [EmployeeAuthGuard] }];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CheckInRoutingModule { }
