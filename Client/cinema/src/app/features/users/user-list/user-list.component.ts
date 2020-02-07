import { ModalMessageComponent } from './../../../shared/modal-message/modal-message.component';
import { ModalComponent } from './../../../shared/modal/modal.component';
import { User } from './../shared/user.model';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from './../user.service';
import { UserDataSource } from './../user.datasource';
import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { MatPaginator, MatDialog } from '@angular/material';
import { tap, debounceTime, distinctUntilChanged, take } from 'rxjs/operators';
import { fromEvent } from 'rxjs';

@Component({
    selector: 'app-user-list',
    templateUrl: './user-list.component.html',
    styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit, AfterViewInit {
    private dataCount: number;
    private dataSource: UserDataSource;
    private displayedColumns: string[] = ['id', 'name', 'email', 'permissionLevel', 'edit', 'delete'];
    private error: string;

    private permissionDisplayName = {
        1: "Gerente",
        2: "Atendente",
        3: "Cliente"
    };

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild('input', { static: false }) input: ElementRef;

    constructor(private userService: UserService, private router: Router, private activeRoute: ActivatedRoute, private dialog: MatDialog) { }

    ngOnInit() {
        this.dataSource = new UserDataSource(this.userService);
        this.dataSource.loadUsers();
        this.dataSource.count$.subscribe(x => this.dataCount = x);
    }

    ngAfterViewInit() {
        fromEvent(this.input.nativeElement, 'keyup')
            .pipe(
                debounceTime(150),
                distinctUntilChanged(),
                tap(() => {
                    this.paginator.pageIndex = 0;
                    this.loadUsersPage();
                })
            )
            .subscribe();

        this.paginator.page
            .pipe(
                tap(() => this.loadUsersPage())
            )
            .subscribe();
    }

    loadUsersPage() {
        this.dataSource.loadUsers(
            this.input.nativeElement.value,
            'id asc',
            this.paginator.pageIndex * this.paginator.pageSize,
            this.paginator.pageSize);
    }

    update(user: User) {
        this.router.navigate([`./editar/${user.id}`], { relativeTo: this.activeRoute });
    }

    remove(user: User){
        const dialogRef = this.dialog.open(ModalComponent, {
            width: '400px',
            data: { title: "Remover Usuário", message: `Deseja remover o usuário ${user.name}?` }
        });

        dialogRef.afterClosed().subscribe(res => {
            if (res) {
                this.userService.remove(user.id).pipe(take(1)).subscribe(removed => {
                    if (removed) {
                        const okDialog = this.dialog.open(ModalMessageComponent, {
                            width: '400px',
                            data: { title: "Remover Usuário", message: `Usuário ${user.name} removido com sucesso!` }
                        }).afterClosed().subscribe(() => {
                            this.loadUsersPage();
                        });
                        this.error = '';
                    } else {
                        this.error = 'Eita.... não foi......';
                    }
                }, error => {
                    this.error = error;
                });
            }
        });
    }

    addUser() {
        this.router.navigate(['./cadastrar'], { relativeTo: this.activeRoute });
    }
}
