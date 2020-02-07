import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {NgxMaterialTimepickerModule} from 'ngx-material-timepicker';


import { MovieService } from './../movies/movie.service';
import { MaterialModule } from './../../shared/material/material.module';
import { AdminAuthGuard } from './../../shared/guards/admin-auth.guard';
import { SessionRegisterFormComponent } from './session-register-form/session-register-form.component';
import { SessionListComponent } from './session-list/session-list.component';
import { SessionsRoutingModule } from './sessions-routing.module';


@NgModule({
    declarations: [SessionListComponent, SessionRegisterFormComponent],
    imports: [
        CommonModule,
        SessionsRoutingModule,
        MaterialModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule,
        MaterialModule,
        NgxMaterialTimepickerModule
    ],
    providers: [AdminAuthGuard, MovieService]
})
export class SessionsModule { }
