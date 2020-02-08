import { CookieHelperService } from './../../shared/cookie-helper/cookie-helper.service';
import { Component, OnInit } from '@angular/core';
import { AuthService } from './auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { User, Permission } from '../users/shared/user.model';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
    private user: User = new User();
    loading = false;
    error = "";

    constructor(private router: Router, private route: ActivatedRoute, private cookie: CookieHelperService, private authService: AuthService) { }

    ngOnInit() {
        if (this.authService.permissionLevel() != Permission.none) {
            this.router.navigateByUrl('/');
        }
    }

    login() {
        this.loading = true;
        this.authService.login(this.user).subscribe((x: any) => {
            const accessToken = x.access_token;
            this.cookie.createCookie("token", accessToken, 1);
            this.authService.getPermission(this.user);
            this.loading = false;
        },
        erro => {
            this.error = erro;
            this.loading = false;
        });
    }
}
