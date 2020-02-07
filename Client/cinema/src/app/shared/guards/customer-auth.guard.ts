import { Permission } from './../../features/users/shared/user.model';
import { AuthService } from '../../features/login/auth.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class CustomerAuthGuard implements CanActivate {

	constructor(
		private authService: AuthService,
		private router: Router) { }

	canActivate(
		route: ActivatedRouteSnapshot,
		state: RouterStateSnapshot):
		Observable<boolean> | boolean {

		if (this.authService.permissionLevel() == Permission.customer){
			return true;
		}
		this.router.navigate(['/login']);
		return false;
	}
}
