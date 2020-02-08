import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../shared/material/material.module';
import { AdminAuthGuard } from './../../shared/guards/admin-auth.guard';
import { MovieRegisterFormComponent } from './movie-register-form/movie-register-form.component';
import { MovieUpdateFormComponent } from './movie-update-form/movie-update-form.component';
import { MovieListComponent } from './movie-list/movie-list.component';
import { MoviesRoutingModule } from './movies-routing.module';
import { MovieResolveService } from './movie.service';


@NgModule({
    declarations: [MovieListComponent, MovieRegisterFormComponent, MovieUpdateFormComponent],
    imports: [
        CommonModule,
        MoviesRoutingModule,
        MaterialModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule
    ],
    providers: [AdminAuthGuard, MovieResolveService]
})
export class MoviesModule { }
