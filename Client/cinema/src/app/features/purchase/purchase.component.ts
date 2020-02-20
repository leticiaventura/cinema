import { map, take } from 'rxjs/operators';
import { Snack } from './../snacks/shared/snack.model';
import { PurchaseService } from './purchase.service';
import { SnackService } from './../snacks/snack.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material';
import { Session } from '../sessions/shared/session.model';
import { ModalComponent } from 'src/app/shared/modal/modal.component';
import { ModalMessageComponent } from 'src/app/shared/modal-message/modal-message.component';

@Component({
    selector: 'app-purchase',
    templateUrl: './purchase.component.html',
    styleUrls: ['./purchase.component.scss']
})
export class PurchaseComponent implements OnInit {

    formGroup: FormGroup;
    freeSeats: number;
    session: any;
    snacks: [];
    error: string;

    dates = [];
    private audioDisplay = {
        0: "Leg",
        1: "Dub"
    };

    private animationDisplay = {
        0: "2D",
        1: "3D"
    }


    constructor(public fb: FormBuilder, private purchaseService: PurchaseService, private router: Router, private activeRoute: ActivatedRoute, private dialog: MatDialog) { }

    ngOnInit(): void {
        this.activeRoute.data.subscribe((data) => {
            this.freeSeats = data.movie.lounge.seats - data.movie.purchasedSeats;
            this.session = data.movie;
            this.reactiveForm();
            this.loadSnacks();
        });

    }

    reactiveForm() {
        this.formGroup = this.fb.group({
            sessionId: [this.session.id, [Validators.required]],
            seats: [1, [Validators.required, Validators.min(1), Validators.max(this.freeSeats)]],
            snacksArray: this.fb.array([])
        })
    }

    submitForm() {
        console.log(this.formGroup.value);

        const dialogRef = this.dialog.open(ModalComponent, {
            width: '400px',
            data: { title: "Confirmar Compra", message: `Deseja finalizar a compra para o filme ${this.session.movie.name}?` }
        }).afterClosed().subscribe(res => {
            if (res) {
                this.purchaseService.add(this.formGroup.value).pipe(take(1)).subscribe(
                    success => {
                        const okDialog = this.dialog.open(ModalMessageComponent, {
                            width: '400px',
                            data: { title: "Confirmar Compra", message: `Compra para ${this.session.movie.name} finalizada com sucesso!` }
                        }).afterClosed().subscribe(() => {
                            this.router.navigate(['../../'], { relativeTo: this.activeRoute });
                        });
                    }, error => {
                        this.error = error;
                    });
            }
        });
    }

    updateSnacks() {
        let control = <FormArray>this.formGroup.get('snacksArray');
        while (control.length !== 0) {
            control.removeAt(0)
        }
        this.snacks.filter((snack: any) => snack.quantity).forEach((x: any) => {
            control.push(this.fb.group({
                name: x.name,
                price: x.price,
                quantity: x.quantity
            }))
        });
    }

    loadSnacks() {
        this.purchaseService.findAllSnacks().subscribe((x: any) => this.snacks = x.map(obj => ({ ...obj, quantity: 0 })))
    }

    errorHandling = (control: string, error: string) => {
        return this.formGroup.controls[control].hasError(error);
    }

    add(snack) {
        snack.quantity++;
    }

    remove(snack) {
        if (snack.quantity) {
            snack.quantity--;
        }
    }

    totalPrice() {
        var total = this.formGroup.value.snacksArray.length ? this.formGroup.value.snacksArray.map(snack => snack.quantity * snack.price).reduce((acc, cur) => acc + cur) : 0;
        total += this.session.price * this.formGroup.value.seats;
        return total;
    }
}

