import { PurchasedSessions } from './../history/history.model';
import { catchError } from 'rxjs/operators';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { SessionService } from './../sessions/session.service';
import { Session } from './../sessions/shared/session.model';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Snack } from '../snacks/shared/snack.model';
import { API } from 'src/main-config';
import { PurchasedTickets } from '../check-in/check-in.model';

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
        return this.http.post(this.api + 'api/purchases', data).pipe(
            catchError(this.error)
        );
    }

    findAllPurchasesByUser(filter = '', skip = 0, top = 10): Observable<PurchasedSessions> {
        var param = new HttpParams()
            .set('$skip', skip.toString())
            .set('$top', top.toString())
            .set('$count', 'true');

        if (filter) {
            param = param.set('$filter', filter)
        }
        return this.http.get<PurchasedSessions>(this.api + 'api/purchases', {
            params: param
        });
    }

    findAllTickets(filterName = '', filterDate = '', skip = 0, top = 10): Observable<PurchasedTickets> {
        var param = new HttpParams()
            .set('$skip', skip.toString())
            .set('$top', top.toString())
            .set('$count', 'true');

        if (filterName) {
            param = param.set('$filterName', filterName)
        }
        if (filterDate){
            param = param.set('$filterDate', filterDate)
        } else {
            param = param.set('$filterDate', new Date().toISOString())
        }
        return this.http.get<PurchasedTickets>(this.api + 'api/purchases/check-in', {
            params: param
        });
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
