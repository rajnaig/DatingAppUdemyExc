import { Component, EventEmitter, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-delete-photo-modal',
  templateUrl: './delete-photo-modal.component.html',
  styleUrls: ['./delete-photo-modal.component.css']
})
export class DeletePhotoModalComponent {
  @Output() confirm = new EventEmitter<boolean>();

  constructor(private bsModalRef: BsModalRef) {}

  closeAndConfirm(value: boolean) {
    this.confirm.emit(value);
    this.bsModalRef.hide();
  }
}
