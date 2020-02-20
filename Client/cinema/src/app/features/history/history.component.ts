import { PurchaseService } from './../purchase/purchase.service';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { SessionHistoryDataSource } from './history.datasource';
import { MatPaginator } from '@angular/material';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss']
})
export class HistoryComponent implements OnInit {

    private dataCount: number;
    private dataSource: SessionHistoryDataSource;
    private displayedColumns: string[] = ['movie', 'lounge', 'date'];

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild('input', { static: false }) input: ElementRef;

    constructor(private purchaseService: PurchaseService) { }

    ngOnInit() {
        this.dataSource = new SessionHistoryDataSource(this.purchaseService);
        this.dataSource.loadSessionHistory();
        this.dataSource.count$.subscribe(x => this.dataCount = x);
    }

    ngAfterViewInit() {
        fromEvent(this.input.nativeElement, 'keyup')
            .pipe(
                debounceTime(150),
                distinctUntilChanged(),
                tap(() => {
                    this.paginator.pageIndex = 0;
                    this.loadSessionHistoryPage();
                })
            )
            .subscribe();

        this.paginator.page
            .pipe(
                tap(() => this.loadSessionHistoryPage())
            )
            .subscribe();
    }

    loadSessionHistoryPage() {
        this.dataSource.loadSessionHistory(
            this.input.nativeElement.value,
            this.paginator.pageIndex * this.paginator.pageSize,
            this.paginator.pageSize);
    }    
}
