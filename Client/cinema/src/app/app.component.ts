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
    showNavBar: boolean = false;
    showSideBar: boolean = false;

    constructor(private authService: AuthService) {
    }

    ngOnInit(): void {
        this.processPermission(this.authService.permissionLevel());
        this.authService.applyPermission.subscribe(permission => {
            this.processPermission(permission);
        });
    }

    private processPermission(permission: Permission) {
        switch (permission) {
            case Permission.customer:
                this.showNavBar = true;
                this.showSideBar = false;
                break;
            case Permission.admin:
                this.showSideBar = true;
                this.showNavBar = false;
                break;
            default:
                this.showSideBar = false;
                this.showNavBar = false;
                break;
        }
    }
}
