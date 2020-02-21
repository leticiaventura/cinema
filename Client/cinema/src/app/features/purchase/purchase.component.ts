import { take } from 'rxjs/operators';
import { PurchaseService } from './purchase.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material';
import { ModalComponent } from 'src/app/shared/modal/modal.component';
import { ModalMessageComponent } from 'src/app/shared/modal-message/modal-message.component';
import { SeatStatus } from '../sessions/shared/session.model';

@Component({
    selector: 'app-purchase',
    templateUrl: './purchase.component.html',
    styleUrls: ['./purchase.component.scss']
})
export class PurchaseComponent implements OnInit {

    formGroup: FormGroup;
    session: any;
    snacks: [];
    error: string;
    rows: number[];
    columns: number[];

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
            this.session = data.movie;
            this.reactiveForm();
            this.loadSnacks();
            this.loadSeatSelector();
        });

    }

    reactiveForm() {
        this.formGroup = this.fb.group({
            sessionId: [this.session.id, [Validators.required]],
            snacksArray: this.fb.array([]),
            seatsArray: this.fb.array([], Validators.required)
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
        total += this.session.price * this.formGroup.value.seatsArray.length;
        return total;
    }

    loadSeatSelector() {
        this.session.seats = [];
        this.rows = Array(this.session.lounge.rows).fill(0).map((x, i) => i);
        this.columns = Array(this.session.lounge.columns).fill(0).map((x, i) => i);

        for (var row = 0; row < this.session.lounge.rows; row++) {
            for (var column = 0; column < this.session.lounge.columns; column++) {
                var taken = !!this.session.takenSeats.filter(x => x.row == row && x.column == column)[0];
                this.session.seats.push({
                    row: row,
                    column: column,
                    status: taken ? SeatStatus.taken : SeatStatus.free
                });
            }
        }
    }

    isSeatFree(row, column) {
        return this.getSeat(row, column).status == SeatStatus.free;
    }

    isSeatSelected(row, column) {
        return this.getSeat(row, column).status == SeatStatus.selected;
    }

    isSeatTaken(row, column) {
        return this.getSeat(row, column).status == SeatStatus.taken;
    }

    getSeat(row, column) {
        return this.session.seats.filter(x => x.row == row && x.column == column)[0];
    }

    selectSeat(row, column) {
        var seat = this.getSeat(row, column);
        if (seat.status == SeatStatus.free) {
            seat.status = SeatStatus.selected;
        } else if (seat.status == SeatStatus.selected) {
            seat.status = SeatStatus.free;
        }
    }

    showSeatsError() {
        return !this.session.seats.filter((seat: any) => seat.status == SeatStatus.selected)[0];
    }

    updateSeats() {
        let control = <FormArray>this.formGroup.get('seatsArray');
        while (control.length !== 0) {
            control.removeAt(0)
        }
        this.session.seats.filter((seat: any) => seat.status == SeatStatus.selected).forEach((x: any) => {
            control.push(this.fb.group({
                row: x.row,
                column: x.column
            }))
        });
    }
}

