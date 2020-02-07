import { Lounge } from './../shared/lounge.model';
import { ModalMessageComponent } from './../../../shared/modal-message/modal-message.component';
import { ModalComponent } from './../../../shared/modal/modal.component';
import { LoungeService } from './../lounge.service';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material';
import { take, map } from 'rxjs/operators';

@Component({
    selector: 'app-lounge-update-form',
    templateUrl: './lounge-update-form.component.html',
    styleUrls: ['./lounge-update-form.component.scss']
})
export class LoungeUpdateFormComponent implements OnInit {
    private requiredAlert: string = "Este campo é obrigatório";
    private error: string;

    formGroup: FormGroup;

    constructor(public fb: FormBuilder, private router: Router, private activeRoute: ActivatedRoute, private loungeService: LoungeService, private dialog: MatDialog) { }

    ngOnInit(): void {
        this.reactiveForm();
        this.activeRoute.data.subscribe(data => {
            this.formGroup.get("id").setValue(data.lounge.id);
            this.formGroup.get("name").setValue(data.lounge.name);
            this.formGroup.get("seats").setValue(data.lounge.seats);
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
            id: ['', [Validators.required]],
            seats: ['', [Validators.required, Validators.min(20), Validators.max(100)]],
        })
    }

    public errorHandling = (control: string, error: string) => {
        return this.formGroup.controls[control].hasError(error);
    }

    submitForm() {
        const dialogRef = this.dialog.open(ModalComponent, {
            width: '400px',
            data: { title: "Editar Sala", message: `Deseja salvar a sala ${this.formGroup.value.name}?` }
        });

        dialogRef.afterClosed().subscribe(res => {
            if (res) {
                this.loungeService.update(this.formGroup.value).pipe(take(1)).subscribe(
                    updated => {
                        if (updated) {
                            const okDialog = this.dialog.open(ModalMessageComponent, {
                                width: '400px',
                                data: { title: "Editar Sala", message: `Sala ${this.formGroup.value.name} atualizada com sucesso!` }
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
        return this.loungeService.checkName(control.value, this.formGroup.value.id).pipe(map(res => {
            return res ? { nameTaken: true } : null;
        }));
    }
}