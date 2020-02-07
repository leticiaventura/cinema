import { MovieService } from './movie.service';
import { Movie, DataGridMovies } from './shared/movie.model';
import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { CollectionViewer } from '@angular/cdk/collections';
import { catchError, finalize } from 'rxjs/operators';

export class MovieDataSource implements DataSource<Movie> {

    private moviesSubject = new BehaviorSubject<Movie[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private moviesCount = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public count$ = this.moviesCount.asObservable();

    constructor(private movieService: MovieService) { }

    connect(collectionViewer: CollectionViewer): Observable<Movie[]> {
        return this.moviesSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
        this.moviesSubject.complete();
        this.loadingSubject.complete();
        this.moviesCount.complete();
    }

    loadMovies(filter = '', orderBy = 'id asc', skip = 0, top = 10) {

        this.loadingSubject.next(true);

        this.movieService.findAllMovies(filter, orderBy, skip, top)
            .pipe(
                catchError(() => of([])),
                finalize(() => this.loadingSubject.next(false))
            )
            .subscribe((dataGridMovies: DataGridMovies) => {
                this.moviesSubject.next(dataGridMovies.items);
                this.moviesCount.next(dataGridMovies.count)
            });
    }
}