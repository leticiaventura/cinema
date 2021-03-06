import { map, catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Permission } from './../users/shared/user.model';
import { CookieHelperService } from './../../shared/cookie-helper/cookie-helper.service';
import { Injectable, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../users/shared/user.model';
import { API } from 'src/main-config';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private permission: Permission = Permission.none;

    applyPermission = new EventEmitter<Permission>();

    constructor(private router: Router, private cookie: CookieHelperService, private httpClient: HttpClient) { }

    login(user: User): Observable<any> {
        let grant_type = 'password';
        let body = `grant_type=${grant_type}&username=${user.email}&password=${user.password}`;

        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/x-www-form-urlencoded'
            })
        };
        return this.httpClient.post(`${API}token`, body, httpOptions).pipe(
            catchError(this.error)
        );
    }

    error(error: HttpErrorResponse) {
        return throwError(error.error.error_description);
    }

    getPermission(user) {
        this.httpClient.get(`${API}api/users/role`).subscribe((x: any) => {
            this.permission = x;
            this.applyPermission.emit(this.permission);
            this.router.navigate(['/']);
            user.type = this.permission;
            this.cookie.createCookie("user_mail", user.email, 1);
            this.cookie.createCookie("user_level", user.type, 1);
        })
    }

    permissionLevel() {
        if (this.permission == Permission.none) {
            var userLevel = this.cookie.readCookie("user_level");
            if (userLevel) {
                this.permission = Number(userLevel);
                this.applyPermission.emit(this.permission);
            }
        }
        return this.permission;
    }
}
