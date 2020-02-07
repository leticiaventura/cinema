import { DataGridMovies } from './../../movies/shared/movie.model';
import { MovieService } from './../../movies/movie.service';
import { ModalMessageComponent } from './../../../shared/modal-message/modal-message.component';
import { SessionService } from './../session.service';
import { Component, OnInit, OnChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from "@angular/forms";
import { Router, ActivatedRoute } from '@angular/router';
import { take, map } from 'rxjs/operators';
import { MatDialog } from '@angular/material';
import { ModalComponent } from './../../../shared/modal/modal.component';


@Component({
    selector: 'app-session-register-form',
    templateUrl: './session-register-form.component.html',
    styleUrls: ['./session-register-form.component.scss']
})
export class SessionRegisterFormComponent implements OnInit {

    private requiredAlert: string = "Este campo é obrigatório";
    private error: string;

    private movieOptions;
    private loungeOptions;

    private movielength = 0;

    private formGroup: FormGroup;

    constructor(public fb: FormBuilder, private router: Router,
        private activeRoute: ActivatedRoute,
        private sessionService: SessionService,
        private movieService: MovieService,
        private dialog: MatDialog) { }

    ngOnInit(): void {
        this.reactiveForm();
        this.loadMovies();
        this.processDate(this.formGroup);
        this.formGroup.get('loungeId').disable();
        this.onValueChanges();
    }

    onValueChanges(): void {
        this.formGroup.get('movie').valueChanges.subscribe(val => {
            this.movielength = val.length;
            this.formGroup.get('movieId').setValue(val.id);
            this.formGroup.get('loungeId').enable();
            this.resetLounges();
        });

        this.formGroup.get('date').valueChanges.subscribe(() => {
            this.processDate(this.formGroup);
            this.resetLounges();
        })

        this.formGroup.get('time').valueChanges.subscribe(() => {
            this.processDate(this.formGroup);
            this.resetLounges();
        })
    }

    getHours(): number {
        return this.formGroup.get('time').value.split(":")[0];
    }

    getMinutes(): number {
        return this.formGroup.get('time').value.split(":")[1];
    }

    loadMovies(): void {
        this.movieService.getAll().subscribe(x => {
            this.movieOptions = x.items;
        });
    }

    loadLounges(): void {
        this.sessionService.getAvailableLounges(this.formGroup.get('date').value, this.movielength).subscribe(x => {
            this.loungeOptions = x;
        });
    }

    resetLounges(): void {
        this.loadLounges();
        this.formGroup.get('loungeId').reset();
    }

    processDate(form: FormGroup) {
        form.get('date').value.setHours(this.getHours());
        form.get('date').value.setMinutes(this.getMinutes());
    }

    reactiveForm() {
        this.formGroup = this.fb.group({
            date: [new Date(), [Validators.required]],
            time: ['16:00', [Validators.required]],
            movieId: ['', [Validators.required]],
            movie: ['', [Validators.required]],
            loungeId: ['', [Validators.required]],
            price: ['', [Validators.required]],
        })
    }

    public errorHandling = (control: string, error: string) => {
        return this.formGroup.controls[control].hasError(error);
    }

    submitForm() {
        const dialogRef = this.dialog.open(ModalComponent, {
            width: '400px',
            data: { title: "Adicionar Sessão", message: `Deseja adicionar a sessão ${this.formGroup.value.movie.name}?` }
        }).afterClosed().subscribe(res => {
            if (res) {
                this.sessionService.add(this.formGroup.value).pipe(take(1)).subscribe(
                    success => {
                        const okDialog = this.dialog.open(ModalMessageComponent, {
                            width: '400px',
                            data: { title: "Adicionar Sessão", message: `Sessão ${this.formGroup.value.movie.name} adicionada com sucesso!` }
                        }).afterClosed().subscribe(() => {
                            this.router.navigate(['../'], { relativeTo: this.activeRoute });
                        });
                    }, error => {
                        this.error = error;
                    });
            }
        });

    }

    cancel() {
        this.router.navigate(['../'], { relativeTo: this.activeRoute });
    }
}