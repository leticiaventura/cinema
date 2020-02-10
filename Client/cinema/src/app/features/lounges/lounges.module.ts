import { LoungeResolveService } from './lounge.service';
import { AdminAuthGuard } from './../../shared/guards/admin-auth.guard';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './../../shared/material/material.module';
import { LoungeUpdateFormComponent } from './lounge-update-form/lounge-update-form.component';
import { LoungeRegisterFormComponent } from './lounge-register-form/lounge-register-form.component';
import { LoungeListComponent } from './lounge-list/lounge-list.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoungesRoutingModule } from './lounges-routing.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';


@NgModule({
    declarations: [LoungeListComponent, LoungeRegisterFormComponent, LoungeUpdateFormComponent],
    imports: [
        CommonModule,
        LoungesRoutingModule,
        MaterialModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule
    ],
    providers: [AdminAuthGuard, LoungeResolveService]
})
export class LoungesModule { }
