import { ModalData } from './../modal/modal.model';
import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'app-modal-message',
    templateUrl: './modal-message.component.html',
    styleUrls: ['./modal-message.component.scss']
})
export class ModalMessageComponent {

    constructor(public dialogRef: MatDialogRef<ModalMessageComponent>, @Inject(MAT_DIALOG_DATA) public data: ModalData) { }

    onNoClick(): void {
        this.dialogRef.close();
    }

}
