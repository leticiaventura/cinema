import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MovieReportDataSource } from './movie-report.datasource';
import { MatPaginator } from '@angular/material';
import { PurchaseService } from '../purchase/purchase.service';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';

@Component({
  selector: 'app-movie-report',
  templateUrl: './movie-report.component.html',
  styleUrls: ['./movie-report.component.scss']
})
export class MovieReportComponent implements OnInit {
    private dataCount: number;
    private dataSource: MovieReportDataSource;
    private displayedColumns: string[] = ['id', 'name', 'revenue'];

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild('input', { static: false }) input: ElementRef;

    constructor(private purchaseService: PurchaseService) { }

    ngOnInit() {
        this.dataSource = new MovieReportDataSource(this.purchaseService);
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
