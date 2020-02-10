import { catchError } from 'rxjs/operators';
import { Movie, DataGridMovies, MovieAddCommand, MovieUpdateCommand, MovieCheckNameQuery } from './shared/movie.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { API } from 'src/main-config';

@Injectable({
    providedIn: 'root'
})
export class MovieService {

    constructor(private http: HttpClient) { }

    private api = `${API}api/movies`;

    findMovieById(movieId: number): Observable<Movie> {
        return this.http.get<Movie>(`${this.api}/${movieId}`);
    }

    findAllMovies(filter = '', orderBy = 'id asc', skip = 0, top = 10): Observable<DataGridMovies> {
        return this.http.get<DataGridMovies>(this.api, {
            params: new HttpParams()
                .set('$filter', `contains(name, '${filter}') or contains(description, '${filter}')`)
                .set('$orderBy', orderBy)
                .set('$skip', skip.toString())
                .set('$top', top.toString())
                .set('$count', 'true')
        });
    }

    add(data, image): Observable<any> {
        return this.http.post(this.api, new MovieAddCommand(data, image)).pipe(
            catchError(this.error)
        );
    }

    update(data, image): Observable<any> {
        return this.http.patch(this.api, new MovieUpdateCommand(data, image)).pipe(
            catchError(this.error)
        )
    }

    error(error: HttpErrorResponse) {
        let errorMessage = '';
        if (Array.isArray(error.error)) {
            error.error.forEach(i => {
                errorMessage += (i.errorMessage + ". ");
            });
        } else {
            errorMessage = error.error.errorMessage;
        }
        return throwError(errorMessage);
    }

    remove(data): Observable<any> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            }),
            body: data
        };
        return this.http.delete(this.api, httpOptions).pipe(
            catchError(this.error)
        );
    }

    checkName(name, id): Observable<any> {
        return this.http.post(this.api + "/name", new MovieCheckNameQuery(name, id)).pipe(
            catchError(this.error)
        );
    }
}


@Injectable()
export class MovieResolveService implements Resolve<Movie> {
    constructor(private service: MovieService) { }

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<any> | Promise<any> | any {
        return this.service.findMovieById(Number(route.paramMap.get('id')));
    }
}