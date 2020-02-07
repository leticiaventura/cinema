import { ModalMessageComponent } from './../../../shared/modal-message/modal-message.component';
import { UserService } from './../user.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { Router, ActivatedRoute } from '@angular/router';
import { take } from 'rxjs/operators';
import { MatDialog } from '@angular/material';
import { ModalComponent } from './../../../shared/modal/modal.component';


@Component({
    selector: 'app-user-register-form',
    templateUrl: './user-register-form.component.html',
    styleUrls: ['./user-register-form.component.scss']
})
export class UserRegisterFormComponent implements OnInit {

    requiredAlert: String = "Este campo é obrigatório";
    passwordMinSize: String = "O campo deve ter pelo menos 3 caracteres";
    error: String;

    permissions = [{ name: "Gerente", value: 1 }, { name: "Atendente", value: 2 }, { name: "Cliente", value: 3 }]

    formGroup: FormGroup;

    constructor(public fb: FormBuilder, private router: Router, private activeRoute: ActivatedRoute, private userService: UserService, private dialog: MatDialog) { }

    ngOnInit(): void {
        this.reactiveForm()
    }

    reactiveForm() {
        this.formGroup = this.fb.group({
            name: ['', [Validators.required]],
            email: ['', [Validators.required]],
            password: ['', [Validators.minLength(3), Validators.required]],
            permissionLevel: ['', [Validators.required]]
        })
    }

    public errorHandling = (control: string, error: string) => {
        return this.formGroup.controls[control].hasError(error);
    }

    submitForm() {
        const dialogRef = this.dialog.open(ModalComponent, {
            width: '400px',
            data: { title: "Adicionar Usuário", message: `Deseja adicionar o usuário ${this.formGroup.value.name}?` }
        }).afterClosed().subscribe(res => {
            if (res) {
                this.userService.add(this.formGroup.value).pipe(take(1)).subscribe(
                    success => {
                        const okDialog = this.dialog.open(ModalMessageComponent, {
                            width: '400px',
                            data: { title: "Adicionar Usuário", message: `Usuário ${this.formGroup.value.name} adicionado com sucesso!` }
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