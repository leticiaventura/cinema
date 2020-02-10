import { SessionService } from './../sessions/session.service';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Session } from '../sessions/shared/session.model';
import { API } from 'src/main-config';

@Injectable({
    providedIn: 'root'
})
export class InTheatersService {

    private api = `${API}api`;

    constructor(private httpClient: HttpClient) { }

    getSessionsByDate(date): Observable<any> {
        var param = new HttpParams()
        .set('date', new Date(date).toISOString());

        return this.httpClient.get<Session>(`${this.api}/sessions/by-date`, {
            params: param
        }).pipe(
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
}