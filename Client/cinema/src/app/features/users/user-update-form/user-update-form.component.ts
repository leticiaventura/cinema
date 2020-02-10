import { ModalMessageComponent } from './../../../shared/modal-message/modal-message.component';
import { ModalComponent } from './../../../shared/modal/modal.component';
import { UserService } from './../user.service';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material';
import { take, map } from 'rxjs/operators';

@Component({
  selector: 'app-user-update-form',
  templateUrl: './user-update-form.component.html',
  styleUrls: ['./user-update-form.component.scss']
})
export class UserUpdateFormComponent implements OnInit {
    private requiredAlert: string = "Este campo é obrigatório";
    private passwordMinSize: string = "O campo deve ter pelo menos 3 caracteres";
    private permissions = [{ name: "Gerente", value: 1 }, { name: "Atendente", value: 2 }, { name: "Cliente", value: 3 }]
    private error: string;

    formGroup: FormGroup;

    constructor(public fb: FormBuilder, private router: Router, private activeRoute: ActivatedRoute, private userService: UserService, private dialog: MatDialog) { }

    ngOnInit(): void {
        this.reactiveForm();
        this.activeRoute.data.subscribe(data => {
            this.formGroup.get("id").setValue(data.user.id);
            this.formGroup.get("name").setValue(data.user.name);
            this.formGroup.get("password").setValue(data.user.password);
            this.formGroup.get("email").setValue(data.user.email);
        });

    }

    reactiveForm() {
        this.formGroup = this.fb.group({
            name: ['', [Validators.required]],
            id: ['', [Validators.required]],
            email: ['',
            {
                validators: [Validators.required],
                asyncValidators: [this.validateEmail.bind(this)],
                updateOn: 'change'
            }],
            password: ['', [Validators.minLength(3)]],
        })
    }

    public errorHandling = (control: string, error: string) => {
        return this.formGroup.controls[control].hasError(error);
    }

    submitForm() {
        const dialogRef = this.dialog.open(ModalComponent, {
            width: '400px',
            data: { title: "Editar Usuário", message: `Deseja salvar o usuário ${this.formGroup.value.name}?` }
        });

        dialogRef.afterClosed().subscribe(res => {
            if (res) {
                this.userService.update(this.formGroup.value).pipe(take(1)).subscribe(
                    updated => {
                    if (updated) {
                        const okDialog = this.dialog.open(ModalMessageComponent, {
                            width: '400px',
                            data: { title: "Editar Usuário", message: `Usuário ${this.formGroup.value.name} atualizado com sucesso!` }
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

    validateEmail(control: AbstractControl) {
        return this.userService.checkEmail(control.value, this.formGroup.value.id).pipe(map(res => {
            return res ? { emailTaken: true } : null;
        }));
    }
}