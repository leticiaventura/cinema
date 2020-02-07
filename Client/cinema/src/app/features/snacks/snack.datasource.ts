import { SnackService } from './snack.service';
import { Snack, DataGridSnacks } from './shared/snack.model';
import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { CollectionViewer } from '@angular/cdk/collections';
import { catchError, finalize } from 'rxjs/operators';

export class SnackDataSource implements DataSource<Snack> {

    private snacksSubject = new BehaviorSubject<Snack[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private snacksCount = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public count$ = this.snacksCount.asObservable();

    constructor(private snackService: SnackService) { }

    connect(collectionViewer: CollectionViewer): Observable<Snack[]> {
        return this.snacksSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
        this.snacksSubject.complete();
        this.loadingSubject.complete();
        this.snacksCount.complete();
    }

    loadSnacks(filter = '', orderBy = 'id asc', skip = 0, top = 10) {

        this.loadingSubject.next(true);

        this.snackService.findAllSnacks(filter, orderBy, skip, top)
            .pipe(
                catchError(() => of([])),
                finalize(() => this.loadingSubject.next(false))
            )
            .subscribe((dataGridSnacks: DataGridSnacks) => {
                this.snacksSubject.next(dataGridSnacks.items);
                this.snacksCount.next(dataGridSnacks.count)
            });
    }
}