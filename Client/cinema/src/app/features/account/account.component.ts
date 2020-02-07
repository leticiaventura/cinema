import { CookieHelperService } from './../../shared/cookie-helper/cookie-helper.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
	selector: 'app-account',
	templateUrl: './account.component.html',
	styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {

    private email = "";

	constructor(private router: Router, private route: ActivatedRoute, private cookie: CookieHelperService) { }

	ngOnInit() {
        this.email = this.cookie.readCookie("user_mail");
	}

	logout() {
        this.cookie.eraseCookie("user_mail");
        this.cookie.eraseCookie("token");
        this.cookie.eraseCookie("user_level");
        window.location.reload();
	}
}
