import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { HttpClient, HttpParams, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { Lounge, DataGridLounges, LoungeAddCommand, LoungeUpdateCommand, LoungeCheckNameQuery } from './shared/lounge.model';
import { API } from 'src/main-config';

@Injectable({
    providedIn: 'root'
})
export class LoungeService {

    constructor(private http: HttpClient) { }

    private api = `${API}api/lounges`;

    findLoungeById(loungeId: number): Observable<Lounge> {
        return this.http.get<Lounge>(`${this.api}/${loungeId}`);
    }

    findAllLounges(filter = '', orderBy = 'id asc', skip = 0, top = 10): Observable<DataGridLounges> {
        return this.http.get<DataGridLounges>(this.api, {
            params: new HttpParams()
                .set('$filter', `contains(name, '${filter}')`)
                .set('$orderBy', orderBy)
                .set('$skip', skip.toString())
                .set('$top', top.toString())
                .set('$count', 'true')
        });
    }

    add(data): Observable<any> {
        return this.http.post(this.api, new LoungeAddCommand(data)).pipe(
            catchError(this.error)
        );
    }

    update(data): Observable<any> {
        return this.http.patch(this.api, new LoungeUpdateCommand(data)).pipe(
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
        return this.http.post(this.api + "/name", new LoungeCheckNameQuery(name, id)).pipe(
            catchError(this.error)
        );
    }
}


@Injectable()
export class LoungeResolveService implements Resolve<Lounge> {
    constructor(private service: LoungeService) { }

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<any> | Promise<any> | any {
        return this.service.findLoungeById(Number(route.paramMap.get('id')));
    }
}