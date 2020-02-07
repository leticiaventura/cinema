import { ModalMessageComponent } from './../../../shared/modal-message/modal-message.component';
import { ModalComponent } from './../../../shared/modal/modal.component';
import { SnackService } from './../snack.service';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material';
import { take, map } from 'rxjs/operators';

@Component({
    selector: 'app-snack-update-form',
    templateUrl: './snack-update-form.component.html',
    styleUrls: ['./snack-update-form.component.scss']
})
export class SnackUpdateFormComponent implements OnInit {
    private requiredAlert: string = "Este campo é obrigatório";
    private error: string;
    private previewUrl: string | ArrayBuffer = '';

    private formGroup: FormGroup;

    constructor(public fb: FormBuilder, private router: Router, private activeRoute: ActivatedRoute, private snackService: SnackService, private dialog: MatDialog) { }

    ngOnInit(): void {
        this.reactiveForm();
        this.activeRoute.data.subscribe(data => {
            this.formGroup.get("id").setValue(data.snack.id);
            this.formGroup.get("name").setValue(data.snack.name);
            this.formGroup.get("price").setValue(data.snack.price);
            this.previewUrl = data.snack.image;
        });
    }

    reactiveForm() {
        this.formGroup = this.fb.group({
            name: ['',
                {
                    validators: [Validators.required],
                    asyncValidators: [this.validateName.bind(this)],
                    updateOn: 'change'
                }],
            price: ['', [Validators.required]],
            id: ['', [Validators.required]]
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
            data: { title: "Editar Snack", message: `Deseja salvar o snack ${this.formGroup.value.name}?` }
        });

        dialogRef.afterClosed().subscribe(res => {
            if (res) {
                this.snackService.update(this.formGroup.value, this.previewUrl).pipe(take(1)).subscribe(
                    updated => {
                        if (updated) {
                            const okDialog = this.dialog.open(ModalMessageComponent, {
                                width: '400px',
                                data: { title: "Editar Snack", message: `Snack ${this.formGroup.value.name} atualizado com sucesso!` }
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
        return this.snackService.checkName(control.value, this.formGroup.value.id).pipe(map(res => {
            return res ? { nameTaken: true } : null;
        }));
    }
}