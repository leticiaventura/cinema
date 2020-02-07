import { LoungeResolveService } from './lounge.service';
import { LoungeUpdateFormComponent } from './lounge-update-form/lounge-update-form.component';
import { LoungeRegisterFormComponent } from './lounge-register-form/lounge-register-form.component';
import { AdminAuthGuard } from './../../shared/guards/admin-auth.guard';
import { LoungeListComponent } from './lounge-list/lounge-list.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        component: LoungeListComponent,
        canActivate: [AdminAuthGuard]
    },
    {
        path: 'cadastrar',
        component: LoungeRegisterFormComponent
    },
    {
        path: 'editar/:id',
        component: LoungeUpdateFormComponent,
        resolve: {
            lounge: LoungeResolveService
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LoungesRoutingModule { }
