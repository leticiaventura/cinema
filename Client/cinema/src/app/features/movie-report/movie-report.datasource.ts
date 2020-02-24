import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { CollectionViewer } from '@angular/cdk/collections';
import { catchError, finalize } from 'rxjs/operators';
import { MovieReport, DataGridMovieReport } from '../movies/shared/movie.model';
import { PurchaseService } from '../purchase/purchase.service';

export class MovieReportDataSource implements DataSource<MovieReport> {

    private sessionHistorysSubject = new BehaviorSubject<MovieReport[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private count = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public count$ = this.count.asObservable();

    constructor(private purchaseService: PurchaseService) { }

    connect(collectionViewer: CollectionViewer): Observable<MovieReport[]> {
        return this.sessionHistorysSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
        this.sessionHistorysSubject.complete();
        this.loadingSubject.complete();
        this.count.complete();
    }

    loadSessionHistory(filter = '', skip = 0, top = 10) {

        this.loadingSubject.next(true);

        this.purchaseService.getMovieReport(filter, skip, top)
            .pipe(
                catchError(() => of([])),
                finalize(() => this.loadingSubject.next(false))
            )
            .subscribe((purchasedSessions: DataGridMovieReport) => {
                this.sessionHistorysSubject.next(purchasedSessions.items);
                this.count.next(purchasedSessions.count)
            });
    }
}