import { ModalMessageComponent } from './../../../shared/modal-message/modal-message.component';
import { LoungeService } from './../lounge.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from "@angular/forms";
import { Router, ActivatedRoute } from '@angular/router';
import { take, map } from 'rxjs/operators';
import { MatDialog } from '@angular/material';
import { ModalComponent } from './../../../shared/modal/modal.component';


@Component({
    selector: 'app-lounge-register-form',
    templateUrl: './lounge-register-form.component.html',
    styleUrls: ['./lounge-register-form.component.scss']
})
export class LoungeRegisterFormComponent implements OnInit {

    requiredAlert: String = "Este campo é obrigatório";
    error: String;

    formGroup: FormGroup;

    constructor(public fb: FormBuilder, private router: Router, private activeRoute: ActivatedRoute, private loungeService: LoungeService, private dialog: MatDialog) { }

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
            seats: ['', [Validators.required, Validators.min(20), Validators.max(100)]],
        })
    }

    public errorHandling = (control: string, error: string) => {
        return this.formGroup.controls[control].hasError(error);
    }

    submitForm() {
        const dialogRef = this.dialog.open(ModalComponent, {
            width: '400px',
            data: { title: "Adicionar Sala", message: `Deseja adicionar a sala ${this.formGroup.value.name}?` }
        }).afterClosed().subscribe(res => {
            if (res) {
                this.loungeService.add(this.formGroup.value).pipe(take(1)).subscribe(
                    success => {
                        const okDialog = this.dialog.open(ModalMessageComponent, {
                            width: '400px',
                            data: { title: "Adicionar Sala", message: `Sala ${this.formGroup.value.name} adicionada com sucesso!` }
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
        return this.loungeService.checkName(control.value, this.formGroup.value.id).pipe(map(res => {
            return res ? { nameTaken: true } : null;
        }));
    }

}