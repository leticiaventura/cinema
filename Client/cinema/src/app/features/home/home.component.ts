import { Permission } from './../users/shared/user.model';
import { AuthService } from './../login/auth.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

    constructor(private router: Router, private route: ActivatedRoute, private authService: AuthService) { }

    ngOnInit() {
        switch (this.authService.permissionLevel()) {
            case Permission.admin: this.router.navigateByUrl('/usuarios');
                break;
            case Permission.customer: this.router.navigateByUrl('/em-cartaz');
                break;
            case Permission.employee: this.router.navigateByUrl('/disponibilidade-sessoes');
                break;
            default: this.router.navigateByUrl('/login');
        }
    }

}
