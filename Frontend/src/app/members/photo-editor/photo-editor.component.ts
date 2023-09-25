import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Photo } from 'src/app/_models/photo';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { environment } from 'src/environments/environment';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { DeletePhotoModalComponent } from 'src/app/modals/delete-photo-modal/delete-photo-modal.component';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css'],
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: Member | undefined;
  uploader: FileUploader | undefined;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  user: User | undefined;
  bsModalRef: BsModalRef | undefined;

  constructor(
    private accountService: AccountService,
    private memberService: MembersService,
    private modalService: BsModalService
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe((user) => {
      if (user) this.user = user;
    });
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.user?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo = JSON.parse(response);
        this.member?.photos.push(photo);
        if (photo.isMain && this.member && this.user) {
          this.user.photoUrl = photo.url;
          this.member.photoUrl = photo.url;
          this.accountService.setCurrentuser(this.user);
        }
      }
    };
  }

  SetMainPhoto(photo: Photo) {
    this.memberService.SetMainPhoto(photo.id).subscribe({
      next: () => {
        if (this.user && this.member) {
          this.user.photoUrl = photo.url;
          this.accountService.setCurrentuser(this.user);
          this.member.photoUrl = photo.url;
          this.member.photos.forEach((p) => {
            if (p.isMain) p.isMain = false;
            if (p.id === photo.id) p.isMain = true;
          });
        }
      },
    });
  }

  DeletePhoto(photo: Photo) {
    this.bsModalRef = this.modalService.show(DeletePhotoModalComponent);
    this.bsModalRef.content.confirm.subscribe((result: boolean) => {
      if (result) {
        this.memberService.DeletePhoto(photo.id).subscribe({
          next: (_) => {
            if (this.member) {
              this.member.photos = this.member.photos.filter(
                (x) => x.id !== photo.id
              );
              this.bsModalRef?.hide();
            }
          },
        });
      }
    });
  }
}
