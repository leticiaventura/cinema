import { PurchasedSession as PurchasedSession, PurchasedSessions } from './history.model';
import { PurchaseService } from './../purchase/purchase.service';
import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { CollectionViewer } from '@angular/cdk/collections';
import { catchError, finalize } from 'rxjs/operators';

export class SessionHistoryDataSource implements DataSource<PurchasedSession> {

    private sessionHistorysSubject = new BehaviorSubject<PurchasedSession[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private purchasedSessionsCount = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public count$ = this.purchasedSessionsCount.asObservable();

    constructor(private purchaseService: PurchaseService) { }

    connect(collectionViewer: CollectionViewer): Observable<PurchasedSession[]> {
        return this.sessionHistorysSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
        this.sessionHistorysSubject.complete();
        this.loadingSubject.complete();
        this.purchasedSessionsCount.complete();
    }

    loadSessionHistory(filter = '', skip = 0, top = 10) {

        this.loadingSubject.next(true);

        this.purchaseService.findAllPurchasesByUser(filter, skip, top)
            .pipe(
                catchError(() => of([])),
                finalize(() => this.loadingSubject.next(false))
            )
            .subscribe((purchasedSessions: PurchasedSessions) => {
                this.sessionHistorysSubject.next(purchasedSessions.items);
                this.purchasedSessionsCount.next(purchasedSessions.count)
            });
    }
}