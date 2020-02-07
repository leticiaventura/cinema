import { MovieUpdateFormComponent } from './movie-update-form/movie-update-form.component';
import { MovieResolveService } from './movie.service';
import { MovieRegisterFormComponent } from './movie-register-form/movie-register-form.component';
import { MovieListComponent } from './movie-list/movie-list.component';
import { AdminAuthGuard } from './../../shared/guards/admin-auth.guard';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        component: MovieListComponent,
        canActivate: [AdminAuthGuard]
    },
    {
        path: 'cadastrar',
        component: MovieRegisterFormComponent
    },
    {
        path: 'editar/:id',
        component: MovieUpdateFormComponent,
        resolve: {
            movie: MovieResolveService
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class MoviesRoutingModule { }
