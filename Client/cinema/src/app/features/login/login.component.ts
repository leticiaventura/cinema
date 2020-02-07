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

	constructor(private router: Router, private route: ActivatedRoute, private authService: AuthService) { }

	ngOnInit() {
        if (this.authService.permissionLevel() != Permission.none) {
            this.router.navigateByUrl('/');
        }
	}

	login() {
		this.authService.login(this.user);
	}
}
