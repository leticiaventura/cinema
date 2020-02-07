import { ModalMessageComponent } from './../../../shared/modal-message/modal-message.component';
import { ModalComponent } from './../../../shared/modal/modal.component';
import { Session } from './../shared/session.model';
import { ActivatedRoute, Router } from '@angular/router';
import { SessionService } from './../session.service';
import { SessionDataSource } from './../session.datasource';
import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { MatPaginator, MatDialog } from '@angular/material';
import { tap, debounceTime, distinctUntilChanged, take } from 'rxjs/operators';
import { fromEvent } from 'rxjs';

@Component({
    selector: 'app-session-list',
    templateUrl: './session-list.component.html',
    styleUrls: ['./session-list.component.scss']
})
export class SessionListComponent implements OnInit, AfterViewInit {
    private dataCount: number;
    private dataSource: SessionDataSource;
    private displayedColumns: string[] = ['id', 'start', 'end', 'movie', 'lounge', 'animationAudio', 'price', 'delete'];
    private error: string;

    private audioDisplay = {
        0: "Original",
        1: "Dublado"
    };

    private animationDisplay = {
        0: "2D",
        1: "3D"
    }

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild('input', { static: false }) input: ElementRef;

    constructor(private sessionService: SessionService, private router: Router, private activeRoute: ActivatedRoute, private dialog: MatDialog) { }

    ngOnInit() {
        this.dataSource = new SessionDataSource(this.sessionService);
        this.dataSource.loadSessions();
        this.dataSource.count$.subscribe(x => this.dataCount = x);
    }

    ngAfterViewInit() {
        this.paginator.page
            .pipe(
                tap(() => this.loadSessionsPage())
            )
            .subscribe();
    }

    loadSessionsPage() {
        this.dataSource.loadSessions(
            '',
            'start desc',
            this.paginator.pageIndex * this.paginator.pageSize,
            this.paginator.pageSize);
    }

    update(session: Session) {
        this.router.navigate([`./editar/${session.id}`], { relativeTo: this.activeRoute });
    }

    remove(session: Session) {
        const dialogRef = this.dialog.open(ModalComponent, {
            width: '400px',
            data: { title: "Remover Sessão", message: `Deseja remover a sessão ${session.movie}?` }
        });

        dialogRef.afterClosed().subscribe(res => {
            if (res) {
                this.sessionService.remove(session.id).pipe(take(1)).subscribe(removed => {
                    if (removed) {
                        const okDialog = this.dialog.open(ModalMessageComponent, {
                            width: '400px',
                            data: { title: "Remover Sessão", message: `Sessão ${session.movie} removido com sucesso!` }
                        }).afterClosed().subscribe(() => {
                            this.loadSessionsPage();
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

    addSession() {
        this.router.navigate(['./cadastrar'], { relativeTo: this.activeRoute });
    }
}
