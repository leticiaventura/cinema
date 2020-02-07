import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { MatPaginator, MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { tap, debounceTime, distinctUntilChanged, take } from 'rxjs/operators';
import { fromEvent } from 'rxjs';
import { ModalMessageComponent } from './../../../shared/modal-message/modal-message.component';
import { Lounge } from './../shared/lounge.model';
import { LoungeDataSource } from './../lounge.datasource';
import { ModalComponent } from './../../../shared/modal/modal.component';
import { LoungeService } from './../lounge.service';

@Component({
    selector: 'app-lounge-list',
    templateUrl: './lounge-list.component.html',
    styleUrls: ['./lounge-list.component.scss']
})
export class LoungeListComponent implements OnInit, AfterViewInit {
    private dataCount: number;
    private dataSource: LoungeDataSource;
    private displayedColumns: string[] = ['id', 'name', 'seats', 'edit', 'delete'];
    private error: string;

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild('input', { static: false }) input: ElementRef;

    constructor(private loungeService: LoungeService, private router: Router, private activeRoute: ActivatedRoute, private dialog: MatDialog) { }

    ngOnInit() {
        this.dataSource = new LoungeDataSource(this.loungeService);
        this.dataSource.loadLounges();
        this.dataSource.count$.subscribe(x => this.dataCount = x);
    }

    ngAfterViewInit() {
        fromEvent(this.input.nativeElement, 'keyup')
            .pipe(
                debounceTime(150),
                distinctUntilChanged(),
                tap(() => {
                    this.paginator.pageIndex = 0;
                    this.loadLoungesPage();
                })
            )
            .subscribe();

        this.paginator.page
            .pipe(
                tap(() => this.loadLoungesPage())
            )
            .subscribe();
    }

    loadLoungesPage() {
        this.dataSource.loadLounges(
            this.input.nativeElement.value,
            'id asc',
            this.paginator.pageIndex * this.paginator.pageSize,
            this.paginator.pageSize);
    }

    update(lounge: Lounge) {
        this.router.navigate([`./editar/${lounge.id}`], { relativeTo: this.activeRoute });
    }

    remove(lounge: Lounge){
        const dialogRef = this.dialog.open(ModalComponent, {
            width: '400px',
            data: { title: "Remover Sala", message: `Deseja remover a sala ${lounge.name}?` }
        });

        dialogRef.afterClosed().subscribe(res => {
            if (res) {
                this.loungeService.remove(lounge.id).pipe(take(1)).subscribe(removed => {
                    if (removed) {
                        const okDialog = this.dialog.open(ModalMessageComponent, {
                            width: '400px',
                            data: { title: "Remover Sala", message: `Sala ${lounge.name} removida com sucesso!` }
                        }).afterClosed().subscribe(() => {
                            this.loadLoungesPage();
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

    addLounge() {
        this.router.navigate(['./cadastrar'], { relativeTo: this.activeRoute });
    }
}
