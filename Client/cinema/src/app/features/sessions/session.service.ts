import { catchError } from 'rxjs/operators';
import { Session, DataGridSessions, SessionAddCommand, SessionGetAvailableLoungesQuery } from './shared/session.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { DataGridMovies } from '../movies/shared/movie.model';

@Injectable({
    providedIn: 'root'
})
export class SessionService {

    constructor(private http: HttpClient) { }

    private api = "https://localhost:44374/api/sessions";

    findSessionById(sessionId: number): Observable<Session> {
        return this.http.get<Session>(`${this.api}/${sessionId}`);
    }

    findAllSessions(filter = '', orderBy = 'id desc', skip = 0, top = 10): Observable<DataGridSessions> {
        var param = new HttpParams()
            .set('$orderBy', orderBy)
            .set('$skip', skip.toString())
            .set('$top', top.toString())
            .set('$count', 'true');

        if (filter){
            param.set('$filter', `(start eq '${filter}')`)
        }
        return this.http.get<DataGridSessions>(this.api, {
            params: param
        });
    }

    add(data): Observable<any> {
        return this.http.post(this.api, new SessionAddCommand(data)).pipe(
            catchError(this.error)
        );
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

    getAvailableLounges(start, movieLength): Observable<any> {
        return this.http.post(this.api + "/availablelounges", new SessionGetAvailableLoungesQuery(start, movieLength)).pipe(
            catchError(this.error)
        );
    }

    getAllMovies(): Observable<DataGridMovies> {
        return this.http.get<DataGridMovies>("https://localhost:44374/api/movies", {
            params: new HttpParams()
                .set('$orderBy', 'name asc')
        });
    }
}