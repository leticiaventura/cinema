import { ModalMessageComponent } from './../../../shared/modal-message/modal-message.component';
import { ModalComponent } from './../../../shared/modal/modal.component';
import { MovieService } from './../movie.service';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material';
import { take, map } from 'rxjs/operators';

@Component({
    selector: 'app-movie-update-form',
    templateUrl: './movie-update-form.component.html',
    styleUrls: ['./movie-update-form.component.scss']
})
export class MovieUpdateFormComponent implements OnInit {
    private requiredAlert: string = "Este campo é obrigatório";
    private error: string;
    private previewUrl: string | ArrayBuffer = '';

    private animationOptions = [{ name: "2D", value: 0 }, { name: "3D", value: 1 }];
    private audioOptions = [{ name: "Original", value: 0 }, { name: "Dublado", value: 1 }];

    private formGroup: FormGroup;

    constructor(public fb: FormBuilder, private router: Router, private activeRoute: ActivatedRoute, private movieService: MovieService, private dialog: MatDialog) { }

    ngOnInit(): void {
        this.reactiveForm();
        this.activeRoute.data.subscribe(data => {
            this.formGroup.get("id").setValue(data.movie.id);
            this.formGroup.get("name").setValue(data.movie.name);
            this.formGroup.get("description").setValue(data.movie.description);
            this.formGroup.get("length").setValue(data.movie.length);
            this.formGroup.get("audio").setValue(data.movie.audio);
            this.formGroup.get("animation").setValue(data.movie.animation);
            this.previewUrl = data.movie.image;
        });
    }

    reactiveForm() {
        this.formGroup = this.fb.group({
            id: ['', [Validators.required]],
            name: ['',
                {
                    validators: [Validators.required],
                    asyncValidators: [this.validateName.bind(this)],
                    updateOn: 'change'
                }],
            description: ['', [Validators.required]],
            length: ['', [Validators.min(1), Validators.required]],
            audio: [0, [Validators.required]],
            animation: [0, [Validators.required]]
        })
    }

    
    loadImage(event: any) {
        var reader = new FileReader();
        reader.readAsDataURL(event.target.files[0]);
        reader.onload = () => {
            this.previewUrl = reader.result;
        }
    }

    public errorHandling = (control: string, error: string) => {
        return this.formGroup.controls[control].hasError(error);
    }

    submitForm() {
        const dialogRef = this.dialog.open(ModalComponent, {
            width: '400px',
            data: { title: "Editar Filme", message: `Deseja salvar o filme ${this.formGroup.value.name}?` }
        });

        dialogRef.afterClosed().subscribe(res => {
            if (res) {
                this.movieService.update(this.formGroup.value, this.previewUrl).pipe(take(1)).subscribe(
                    updated => {
                        if (updated) {
                            const okDialog = this.dialog.open(ModalMessageComponent, {
                                width: '400px',
                                data: { title: "Editar Filme", message: `Filme ${this.formGroup.value.name} atualizado com sucesso!` }
                            }).afterClosed().subscribe(() => {
                                this.router.navigate(['../../'], { relativeTo: this.activeRoute });
                            });
                        } else {
                            this.error = 'eita.......não foi.....'
                        }
                    }, error => {
                        this.error = error;
                    });
            }
        });

    }

    cancel() {
        this.router.navigate(['../../'], { relativeTo: this.activeRoute });
    }

    
    validateName(control: AbstractControl) {
        return this.movieService.checkName(control.value, this.formGroup.value.id).pipe(map(res => {
            return res ? { nameTaken: true } : null;
        }));
    }
}