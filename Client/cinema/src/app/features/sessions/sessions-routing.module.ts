import { SessionRegisterFormComponent } from './session-register-form/session-register-form.component';
import { SessionListComponent } from './session-list/session-list.component';
import { AdminAuthGuard } from './../../shared/guards/admin-auth.guard';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        component: SessionListComponent,
        canActivate: [AdminAuthGuard]
    },
    {
        path: 'cadastrar',
        component: SessionRegisterFormComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SessionsRoutingModule { }
