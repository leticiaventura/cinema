import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './features/login/login.component';

const routes: Routes = [
    { path: '', loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule) },
    { path: 'usuarios', loadChildren: () => import('./features/users/users.module').then(m => m.UsersModule) },
    { path: 'em-cartaz', loadChildren: () => import('./features/in-theaters/in-theaters.module').then(m => m.InTheatersModule) },
    { path: 'historico', loadChildren: () => import('./features/history/history.module').then(m => m.HistoryModule) },
    { path: 'salas', loadChildren: () => import('./features/lounges/lounges.module').then(m => m.LoungesModule) },
    { path: 'filmes', loadChildren: () => import('./features/movies/movies.module').then(m => m.MoviesModule) },
    { path: 'snacks', loadChildren: () => import('./features/snacks/snacks.module').then(m => m.SnacksModule) },
    { path: 'sessoes', loadChildren: () => import('./features/sessions/sessions.module').then(m => m.SessionsModule) },
    { path: 'conta', loadChildren: () => import('./features/account/account.module').then(m => m.AccountsModule) },
    {
        path: 'login',
        component: LoginComponent
    },
    { path: '**', redirectTo: '/' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
