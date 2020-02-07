import { SessionService } from './session.service';
import { Session, DataGridSessions } from './shared/session.model';
import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { CollectionViewer } from '@angular/cdk/collections';
import { catchError, finalize } from 'rxjs/operators';

export class SessionDataSource implements DataSource<Session> {

    private sessionsSubject = new BehaviorSubject<Session[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private sessionsCount = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public count$ = this.sessionsCount.asObservable();

    constructor(private sessionService: SessionService) { }

    connect(collectionViewer: CollectionViewer): Observable<Session[]> {
        return this.sessionsSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
        this.sessionsSubject.complete();
        this.loadingSubject.complete();
        this.sessionsCount.complete();
    }

    loadSessions(filter = '', orderBy = 'start desc', skip = 0, top = 10) {

        this.loadingSubject.next(true);

        this.sessionService.findAllSessions(filter, orderBy, skip, top)
            .pipe(
                catchError(() => of([])),
                finalize(() => this.loadingSubject.next(false))
            )
            .subscribe((dataGridSessions: DataGridSessions) => {
                this.sessionsSubject.next(dataGridSessions.items);
                this.sessionsCount.next(dataGridSessions.count)
            });
    }
}