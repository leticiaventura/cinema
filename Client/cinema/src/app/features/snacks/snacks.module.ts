import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../shared/material/material.module';
import { AdminAuthGuard } from './../../shared/guards/admin-auth.guard';
import { SnackRegisterFormComponent } from './snack-register-form/snack-register-form.component';
import { SnackUpdateFormComponent } from './snack-update-form/snack-update-form.component';
import { SnackListComponent } from './snack-list/snack-list.component';
import { SnacksRoutingModule } from './snacks-routing.module';
import { SnackResolveService } from './snack.service';


@NgModule({
    declarations: [SnackListComponent, SnackRegisterFormComponent, SnackUpdateFormComponent],
    imports: [
        CommonModule,
        SnacksRoutingModule,
        MaterialModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule,
        MaterialModule
    ],
    providers: [AdminAuthGuard, SnackResolveService]
})
export class SnacksModule { }
