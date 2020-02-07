import { SnackUpdateFormComponent } from './snack-update-form/snack-update-form.component';
import { SnackResolveService } from './snack.service';
import { SnackRegisterFormComponent } from './snack-register-form/snack-register-form.component';
import { SnackListComponent } from './snack-list/snack-list.component';
import { AdminAuthGuard } from './../../shared/guards/admin-auth.guard';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        component: SnackListComponent,
        canActivate: [AdminAuthGuard]
    },
    {
        path: 'cadastrar',
        component: SnackRegisterFormComponent
    },
    {
        path: 'editar/:id',
        component: SnackUpdateFormComponent,
        resolve: {
            snack: SnackResolveService
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SnacksRoutingModule { }
