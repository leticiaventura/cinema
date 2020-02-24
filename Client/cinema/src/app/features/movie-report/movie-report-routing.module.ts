import { AdminAuthGuard } from './../../shared/guards/admin-auth.guard';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MovieReportComponent } from './movie-report.component';

const routes: Routes = [{ path: '', component: MovieReportComponent, canActivate: [AdminAuthGuard] }];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class MovieReportRoutingModule { }
