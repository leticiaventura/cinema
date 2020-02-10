import { catchError } from 'rxjs/operators';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { SessionService } from './../sessions/session.service';
import { Session } from './../sessions/shared/session.model';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Snack } from '../snacks/shared/snack.model';
import { API } from 'src/main-config';

@Injectable({
    providedIn: 'root'
})
export class PurchaseService {

    api = `${API}`;

    constructor(private http: HttpClient) { }

    findAllSnacks(): Observable<Snack> {
        return this.http.get<Snack>(`${this.api}api/snacks/purchase`);
    }

    add(data): Observable<any> {
        return this.http.post(this.api+'api/purchases', data).pipe(
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


@Injectable()
export class PurchaseResolveService implements Resolve<Session> {
    constructor(private service: SessionService) { }

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<any> | Promise<any> | any {
        return this.service.findSessionById(Number(route.paramMap.get('id')));
    }
}
