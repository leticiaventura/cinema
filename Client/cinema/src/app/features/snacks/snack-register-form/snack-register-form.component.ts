import { ModalMessageComponent } from './../../../shared/modal-message/modal-message.component';
import { SnackService } from './../snack.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from "@angular/forms";
import { Router, ActivatedRoute } from '@angular/router';
import { take, map } from 'rxjs/operators';
import { MatDialog } from '@angular/material';
import { ModalComponent } from './../../../shared/modal/modal.component';


@Component({
    selector: 'app-snack-register-form',
    templateUrl: './snack-register-form.component.html',
    styleUrls: ['./snack-register-form.component.scss']
})
export class SnackRegisterFormComponent implements OnInit {

    private requiredAlert: string = "Este campo é obrigatório";
    private error: string;
    private previewUrl: string | ArrayBuffer = '';

    private formGroup: FormGroup;

    constructor(public fb: FormBuilder, private router: Router, private activeRoute: ActivatedRoute, private snackService: SnackService, private dialog: MatDialog) { }

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
            price: ['', [Validators.required]]
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
            data: { title: "Adicionar Snack", message: `Deseja adicionar o snack ${this.formGroup.value.name}?` }
        }).afterClosed().subscribe(res => {
            if (res) {
                this.snackService.add(this.formGroup.value, this.previewUrl).pipe(take(1)).subscribe(
                    success => {
                        const okDialog = this.dialog.open(ModalMessageComponent, {
                            width: '400px',
                            data: { title: "Adicionar Snack", message: `Snack ${this.formGroup.value.name} adicionado com sucesso!` }
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
        return this.snackService.checkName(control.value, this.formGroup.value.id).pipe(map(res => {
            return res ? { nameTaken: true } : null;
        }));
    }

}