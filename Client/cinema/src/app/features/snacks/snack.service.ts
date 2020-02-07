import { catchError } from 'rxjs/operators';
import { Snack, DataGridSnacks, SnackAddCommand, SnackUpdateCommand, SnackCheckNameQuery } from './shared/snack.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class SnackService {

    constructor(private http: HttpClient) { }

    private api = "https://localhost:44374/api/snacks";

    findSnackById(snackId: number): Observable<Snack> {
        return this.http.get<Snack>(`${this.api}/${snackId}`);
    }

    findAllSnacks(filter = '', orderBy = 'id asc', skip = 0, top = 10): Observable<DataGridSnacks> {
        return this.http.get<DataGridSnacks>(this.api, {
            params: new HttpParams()
                .set('$filter', `contains(name, '${filter}')`)
                .set('$orderBy', orderBy)
                .set('$skip', skip.toString())
                .set('$top', top.toString())
                .set('$count', 'true')
        });
    }

    add(data, image): Observable<any> {
        return this.http.post(this.api, new SnackAddCommand(data, image)).pipe(
            catchError(this.error)
        );
    }

    update(data, image): Observable<any> {
        return this.http.patch(this.api, new SnackUpdateCommand(data, image)).pipe(
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
        return this.http.post(this.api+"/name", new SnackCheckNameQuery(name, id)).pipe(
            catchError(this.error)
        );
    }
}


@Injectable()
export class SnackResolveService implements Resolve<Snack> {
    constructor(private service: SnackService) { }

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<any> | Promise<any> | any {
        return this.service.findSnackById(Number(route.paramMap.get('id')));
    }
}