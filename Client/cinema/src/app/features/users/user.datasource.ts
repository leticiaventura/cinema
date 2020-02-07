import { UserService } from './user.service';
import { User, DataGridUsers } from './shared/user.model';
import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { CollectionViewer } from '@angular/cdk/collections';
import { catchError, finalize } from 'rxjs/operators';

export class UserDataSource implements DataSource<User> {

    private usersSubject = new BehaviorSubject<User[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private usersCount = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public count$ = this.usersCount.asObservable();

    constructor(private userService: UserService) { }

    connect(collectionViewer: CollectionViewer): Observable<User[]> {
        return this.usersSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
        this.usersSubject.complete();
        this.loadingSubject.complete();
        this.usersCount.complete();
    }

    loadUsers(filter = '', orderBy = 'id asc', skip = 0, top = 10) {

        this.loadingSubject.next(true);

        this.userService.findAllUsers(filter, orderBy, skip, top)
            .pipe(
                catchError(() => of([])),
                finalize(() => this.loadingSubject.next(false))
            )
            .subscribe((dataGridUsers: DataGridUsers) => {
                this.usersSubject.next(dataGridUsers.items);
                this.usersCount.next(dataGridUsers.count)
            });
    }
}