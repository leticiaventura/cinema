import { Component } from '@angular/core';
import { AuthService } from './features/login/auth.service';
import { Permission } from './features/users/shared/user.model';
import 'hammerjs';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent {
    permission: Permission = Permission.none;

    constructor(private authService: AuthService) {
    }

    ngOnInit(): void {
        this.processPermission(this.authService.permissionLevel());
        this.authService.applyPermission.subscribe(permission => {
            this.processPermission(permission);
        });
    }

    private isCustomer() {
        return this.permission == Permission.customer;
    }

    private isAdmin() {
        return this.permission == Permission.admin;
    }

    private isEmployee() {
        return this.permission == Permission.employee;
    }

    private processPermission(permission: Permission) {
        this.permission = permission;
    }
}
