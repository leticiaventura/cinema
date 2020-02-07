import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../shared/material/material.module';
import { AdminAuthGuard } from './../../shared/guards/admin-auth.guard';
import { UserRegisterFormComponent } from './user-register-form/user-register-form.component';
import { UserUpdateFormComponent } from './user-update-form/user-update-form.component';
import { UserListComponent } from './user-list/user-list.component';
import { UsersRoutingModule } from './users-routing.module';
import { UserResolveService } from './user.service';


@NgModule({
    declarations: [UserListComponent, UserRegisterFormComponent, UserUpdateFormComponent],
    imports: [
        CommonModule,
        UsersRoutingModule,
        MaterialModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule,
        MaterialModule
    ],
    providers: [AdminAuthGuard, UserResolveService]
})
export class UsersModule { }
