import { catchError } from 'rxjs/operators';
import { User, DataGridUsers, UserAddCommand, UserUpdateCommand } from './shared/user.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class UserService {

    constructor(private http: HttpClient) { }

    private api = "https://localhost:44374/api/users";

    findUserById(userId: number): Observable<User> {
        return this.http.get<User>(`${this.api}/${userId}`);
    }

    findAllUsers(filter = '', orderBy = 'id asc', skip = 0, top = 10): Observable<DataGridUsers> {
        return this.http.get<DataGridUsers>(this.api, {
            params: new HttpParams()
                .set('$filter', `contains(name, '${filter}') or contains(email, '${filter}')`)
                .set('$orderBy', orderBy)
                .set('$skip', skip.toString())
                .set('$top', top.toString())
                .set('$count', 'true')
        });
    }

    add(data): Observable<any> {
        return this.http.post(this.api, new UserAddCommand(data)).pipe(
            catchError(this.error)
        );
    }

    update(data): Observable<any> {
        return this.http.patch(this.api, new UserUpdateCommand(data)).pipe(
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
}


@Injectable()
export class UserResolveService implements Resolve<User> {
    constructor(private service: UserService) { }

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<any> | Promise<any> | any {
        return this.service.findUserById(Number(route.paramMap.get('id')));
    }
}