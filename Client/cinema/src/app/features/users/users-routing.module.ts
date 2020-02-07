import { UserUpdateFormComponent } from './user-update-form/user-update-form.component';
import { UserResolveService } from './user.service';
import { UserRegisterFormComponent } from './user-register-form/user-register-form.component';
import { UserListComponent } from './user-list/user-list.component';
import { AdminAuthGuard } from './../../shared/guards/admin-auth.guard';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        component: UserListComponent,
        canActivate: [AdminAuthGuard]
    },
    {
        path: 'cadastrar',
        component: UserRegisterFormComponent
    },
    {
        path: 'editar/:id',
        component: UserUpdateFormComponent,
        resolve: {
            user: UserResolveService
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UsersRoutingModule { }
