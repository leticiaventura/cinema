import { ModalMessageComponent } from './../../../shared/modal-message/modal-message.component';
import { MovieService } from './../movie.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from "@angular/forms";
import { Router, ActivatedRoute } from '@angular/router';
import { take, map } from 'rxjs/operators';
import { MatDialog } from '@angular/material';
import { ModalComponent } from './../../../shared/modal/modal.component';


@Component({
    selector: 'app-movie-register-form',
    templateUrl: './movie-register-form.component.html',
    styleUrls: ['./movie-register-form.component.scss']
})
export class MovieRegisterFormComponent implements OnInit {

    private requiredAlert: string = "Este campo é obrigatório";
    private error: string;
    private previewUrl: string | ArrayBuffer = '';

    private animationOptions = [{ name: "2D", value: 0 }, { name: "3D", value: 1 }];
    private audioOptions = [{ name: "Original", value: 0 }, { name: "Dublado", value: 1 }];

    private formGroup: FormGroup;

    constructor(public fb: FormBuilder, private router: Router, private activeRoute: ActivatedRoute, private movieService: MovieService, private dialog: MatDialog) { }

    ngOnInit(): void {
        this.reactiveForm()
    }

    reactiveForm() {
        this.formGroup = this.fb.group({
            name: ['',
                {
                    validators: [Validators.required],
                    asyncValidators: [this.validateName.bind(this)],
                    updateOn: 'change'
                }],
            description: ['', [Validators.required]],
            length: ['', [Validators.min(1), Validators.required]],
            audio: [0, [Validators.required]],
            animation: [0, [Validators.required]],
        })
    }

    public errorHandling = (control: string, error: string) => {
        return this.formGroup.controls[control].hasError(error);
    }

    loadImage(event: any) {
        var reader = new FileReader();
        reader.readAsDataURL(event.target.files[0]);
        reader.onload = () => {
            this.previewUrl = reader.result;
        }
    }

    submitForm() {
        const dialogRef = this.dialog.open(ModalComponent, {
            width: '400px',
            data: { title: "Adicionar Filme", message: `Deseja adicionar o filme ${this.formGroup.value.name}?` }
        }).afterClosed().subscribe(res => {
            if (res) {
                this.movieService.add(this.formGroup.value, this.previewUrl).pipe(take(1)).subscribe(
                    success => {
                    const okDialog = this.dialog.open(ModalMessageComponent, {
                        width: '400px',
                        data: { title: "Adicionar Filme", message: `Filme ${this.formGroup.value.name} adicionado com sucesso!` }
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

    validateName(control: AbstractControl) {
        return this.movieService.checkName(control.value, this.formGroup.value.id).pipe(map(res => {
            return res ? { nameTaken: true } : null;
        }));
    }

}