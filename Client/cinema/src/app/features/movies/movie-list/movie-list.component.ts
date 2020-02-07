import { ModalMessageComponent } from './../../../shared/modal-message/modal-message.component';
import { ModalComponent } from './../../../shared/modal/modal.component';
import { Movie } from './../shared/movie.model';
import { ActivatedRoute, Router } from '@angular/router';
import { MovieService } from './../movie.service';
import { MovieDataSource } from './../movie.datasource';
import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { MatPaginator, MatDialog } from '@angular/material';
import { tap, debounceTime, distinctUntilChanged, take } from 'rxjs/operators';
import { fromEvent } from 'rxjs';

@Component({
    selector: 'app-movie-list',
    templateUrl: './movie-list.component.html',
    styleUrls: ['./movie-list.component.scss']
})
export class MovieListComponent implements OnInit, AfterViewInit {
    private dataCount: number;
    private dataSource: MovieDataSource;
    private displayedColumns: string[] = ['id', 'name', 'length', 'animation', 'audio', 'edit', 'delete'];
    private error: string;

    private audioDisplay = {
        0: "Original",
        1: "Dublado"
    };

    private animationDisplay = {
        0: "2D",
        1: "3D"
    }

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild('input', { static: false }) input: ElementRef;

    constructor(private movieService: MovieService, private router: Router, private activeRoute: ActivatedRoute, private dialog: MatDialog) { }

    ngOnInit() {
        this.dataSource = new MovieDataSource(this.movieService);
        this.dataSource.loadMovies();
        this.dataSource.count$.subscribe(x => this.dataCount = x);
    }

    ngAfterViewInit() {
        fromEvent(this.input.nativeElement, 'keyup')
            .pipe(
                debounceTime(150),
                distinctUntilChanged(),
                tap(() => {
                    this.paginator.pageIndex = 0;
                    this.loadMoviesPage();
                })
            )
            .subscribe();

        this.paginator.page
            .pipe(
                tap(() => this.loadMoviesPage())
            )
            .subscribe();
    }

    loadMoviesPage() {
        this.dataSource.loadMovies(
            this.input.nativeElement.value,
            'id asc',
            this.paginator.pageIndex * this.paginator.pageSize,
            this.paginator.pageSize);
    }

    update(movie: Movie) {
        this.router.navigate([`./editar/${movie.id}`], { relativeTo: this.activeRoute });
    }

    remove(movie: Movie) {
        const dialogRef = this.dialog.open(ModalComponent, {
            width: '400px',
            data: { title: "Remover Filme", message: `Deseja remover o filme ${movie.name}?` }
        });

        dialogRef.afterClosed().subscribe(res => {
            if (res) {
                this.movieService.remove(movie.id).pipe(take(1)).subscribe(removed => {
                    if (removed) {
                        const okDialog = this.dialog.open(ModalMessageComponent, {
                            width: '400px',
                            data: { title: "Remover Filme", message: `Filme ${movie.name} removido com sucesso!` }
                        }).afterClosed().subscribe(() => {
                            this.loadMoviesPage();
                        });
                        this.error = '';
                    } else {
                        this.error = 'Eita.... não foi......';
                    }
                }, error => {
                    this.error = error;
                });
            }
        });
    }

    addMovie() {
        this.router.navigate(['./cadastrar'], { relativeTo: this.activeRoute });
    }
}
