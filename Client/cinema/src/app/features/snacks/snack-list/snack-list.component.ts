import { ModalMessageComponent } from './../../../shared/modal-message/modal-message.component';
import { ModalComponent } from './../../../shared/modal/modal.component';
import { Snack } from './../shared/snack.model';
import { ActivatedRoute, Router } from '@angular/router';
import { SnackService } from './../snack.service';
import { SnackDataSource } from './../snack.datasource';
import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { MatPaginator, MatDialog } from '@angular/material';
import { tap, debounceTime, distinctUntilChanged, take } from 'rxjs/operators';
import { fromEvent } from 'rxjs';

@Component({
    selector: 'app-snack-list',
    templateUrl: './snack-list.component.html',
    styleUrls: ['./snack-list.component.scss']
})
export class SnackListComponent implements OnInit, AfterViewInit {
    private dataCount: number;
    private dataSource: SnackDataSource;
    private displayedColumns: string[] = ['id', 'name', 'price', 'edit', 'delete'];
    private error: string;

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild('input', { static: false }) input: ElementRef;

    constructor(private snackService: SnackService, private router: Router, private activeRoute: ActivatedRoute, private dialog: MatDialog) { }

    ngOnInit() {
        this.dataSource = new SnackDataSource(this.snackService);
        this.dataSource.loadSnacks();
        this.dataSource.count$.subscribe(x => this.dataCount = x);
    }

    ngAfterViewInit() {
        fromEvent(this.input.nativeElement, 'keyup')
            .pipe(
                debounceTime(150),
                distinctUntilChanged(),
                tap(() => {
                    this.paginator.pageIndex = 0;
                    this.loadSnacksPage();
                })
            )
            .subscribe();

        this.paginator.page
            .pipe(
                tap(() => this.loadSnacksPage())
            )
            .subscribe();
    }

    loadSnacksPage() {
        this.dataSource.loadSnacks(
            this.input.nativeElement.value,
            'id asc',
            this.paginator.pageIndex * this.paginator.pageSize,
            this.paginator.pageSize);
    }

    update(snack: Snack) {
        this.router.navigate([`./editar/${snack.id}`], { relativeTo: this.activeRoute });
    }

    remove(snack: Snack) {
        const dialogRef = this.dialog.open(ModalComponent, {
            width: '400px',
            data: { title: "Remover Snacks", message: `Deseja remover o snack ${snack.name}?` }
        });

        dialogRef.afterClosed().subscribe(res => {
            if (res) {
                this.snackService.remove(snack.id).pipe(take(1)).subscribe(removed => {
                    if (removed) {
                        const okDialog = this.dialog.open(ModalMessageComponent, {
                            width: '400px',
                            data: { title: "Remover Snacks", message: `Snack ${snack.name} removido com sucesso!` }
                        }).afterClosed().subscribe(() => {
                            this.loadSnacksPage();
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

    addSnack() {
        this.router.navigate(['./cadastrar'], { relativeTo: this.activeRoute });
    }
}
