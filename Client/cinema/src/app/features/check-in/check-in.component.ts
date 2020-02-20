import { PurchaseService } from './../purchase/purchase.service';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { SessionCheckInDataSource } from './check-in.datasource';
import { MatPaginator } from '@angular/material';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';

@Component({
    selector: 'app-check-in',
    templateUrl: './check-in.component.html',
    styleUrls: ['./check-in.component.scss']
})
export class CheckInComponent implements OnInit {

    private dataCount: number;
    private dataSource: SessionCheckInDataSource;
    private displayedColumns: string[] = ['email', 'movie', 'date', 'lounge'];
    private date = new Date();


    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild('input', { static: false }) input: ElementRef;
    @ViewChild('inputDate', { static: false }) inputDate: ElementRef;

    constructor(private purchaseService: PurchaseService) { }

    ngOnInit() {
        this.dataSource = new SessionCheckInDataSource(this.purchaseService);
        this.dataSource.loadAllTickets();
        this.dataSource.count$.subscribe(x => this.dataCount = x);
    }

    ngAfterViewInit() {
        fromEvent(this.input.nativeElement, 'keyup')
            .pipe(
                debounceTime(150),
                distinctUntilChanged(),
                tap(() => {
                    this.paginator.pageIndex = 0;
                    this.loadSessionCheckInPage();
                })
            )
            .subscribe();

        this.paginator.page
            .pipe(
                tap(() => this.loadSessionCheckInPage())
            )
            .subscribe();
    }

    dateChanged() {
        this.paginator.pageIndex = 0;
        this.loadSessionCheckInPage();
    }

    loadSessionCheckInPage() {
        this.dataSource.loadAllTickets(
            this.input.nativeElement.value,
            this.date.toISOString(),
            this.paginator.pageIndex * this.paginator.pageSize,
            this.paginator.pageSize);
    }
}
