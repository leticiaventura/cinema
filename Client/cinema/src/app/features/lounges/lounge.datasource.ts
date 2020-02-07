import { DataSource } from '@angular/cdk/table';
import { CollectionViewer } from '@angular/cdk/collections';
import { catchError, finalize } from 'rxjs/operators';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { Lounge, DataGridLounges } from './shared/lounge.model';
import { LoungeService } from './lounge.service';

export class LoungeDataSource implements DataSource<Lounge> {

    private loungesSubject = new BehaviorSubject<Lounge[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private loungesCount = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public count$ = this.loungesCount.asObservable();

    constructor(private loungeService: LoungeService) { }

    connect(collectionViewer: CollectionViewer): Observable<Lounge[]> {
        return this.loungesSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
        this.loungesSubject.complete();
        this.loadingSubject.complete();
        this.loungesCount.complete();
    }

    loadLounges(filter = '', orderBy = 'id asc', skip = 0, top = 10) {

        this.loadingSubject.next(true);

        this.loungeService.findAllLounges(filter, orderBy, skip, top)
            .pipe(
                catchError(() => of([])),
                finalize(() => this.loadingSubject.next(false))
            )
            .subscribe((dataGridLounges: DataGridLounges) => {
                this.loungesSubject.next(dataGridLounges.items);
                this.loungesCount.next(dataGridLounges.count)
            });
    }
}