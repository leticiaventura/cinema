import { SessionTicket as SessionTicket, PurchasedTickets } from './check-in.model';
import { PurchaseService } from './../purchase/purchase.service';
import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { CollectionViewer } from '@angular/cdk/collections';
import { catchError, finalize } from 'rxjs/operators';

export class SessionCheckInDataSource implements DataSource<SessionTicket> {

    private sessionCheckInsSubject = new BehaviorSubject<SessionTicket[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private purchasedSessionsCount = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public count$ = this.purchasedSessionsCount.asObservable();

    constructor(private purchaseService: PurchaseService) { }

    connect(collectionViewer: CollectionViewer): Observable<SessionTicket[]> {
        return this.sessionCheckInsSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
        this.sessionCheckInsSubject.complete();
        this.loadingSubject.complete();
        this.purchasedSessionsCount.complete();
    }

    loadAllTickets(filterName = '', filterDate = '', skip = 0, top = 10) {

        this.loadingSubject.next(true);

        this.purchaseService.findAllTickets(filterName, filterDate, skip, top)
            .pipe(
                catchError(() => of([])),
                finalize(() => this.loadingSubject.next(false))
            )
            .subscribe((purchasedSessions: PurchasedTickets) => {
                this.sessionCheckInsSubject.next(purchasedSessions.items);
                this.purchasedSessionsCount.next(purchasedSessions.count)
            });
    }
}