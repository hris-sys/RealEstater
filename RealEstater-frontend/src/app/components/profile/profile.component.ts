import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgToastService } from 'ng-angular-popup';
import Common from 'src/app/helpers/Common';
import UserMapper from 'src/app/helpers/UserMapper';
import { DisplayUserInfoModel } from 'src/app/models/DisplayUserInfoModel';
import { AuthService } from 'src/app/services/auth.service';
import { ResetPasswordService } from 'src/app/services/reset-password.service';
import { UserStoreService } from 'src/app/services/user-store.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  user: DisplayUserInfoModel = new DisplayUserInfoModel();
  fullName!: string;
  email!: string;
  isEditing: boolean = false;
  editUserForm!: FormGroup;

  imageUrl: string | ArrayBuffer | null = "";

  constructor(private userService: UserService,
    private authService: AuthService,
    private toast: NgToastService,
    private userStorage: UserStoreService,
    private resetPassword: ResetPasswordService) { }

  ngOnInit(): void {
    this.refreshEditGrid();

    this.email = this.authService.getEmailFromToken();

    this.userService.getUserDataByEmail(this.email)
      .subscribe(data => {
        this.user.firstName = data.firstName;
        this.user.lastName = data.lastName;
        this.user.rating = data.rating;
        this.user.landholdings = data.landholdings;
        this.user.phoneNumber = data.phoneNumber;
        this.user.pictureURL = data.pictureURL;
        this.user.websiteURL = data.websiteURL;
        this.fullName = `${this.user.firstName} ${this.user.lastName}`;
        (document.getElementById('spinner') as HTMLElement).style.display = "none";
        (document.getElementById('card') as HTMLElement).style.display = "block";
      });

    this.editUserForm = new FormGroup({
      firstName: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z]+$')]),
      lastName: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z]+$')]),
      websiteURL: new FormControl('', [Validators.required, Validators.pattern(Common.regexForWebsite)]),
      phoneNumber: new FormControl('', [Validators.required, Validators.pattern(Common.regexForPhone)]),
    });
  }

  startEditing() {
    document.getElementById("editBtn")!.className = "col-md-6 text-center";
    this.isEditing = true;

    document.getElementById("firstName")!.removeAttribute("readonly");
    document.getElementById("lastName")!.removeAttribute("readonly");
    document.getElementById("websiteURL")!.removeAttribute("readonly");
    document.getElementById("phoneNumber")!.removeAttribute("readonly");
  }

  cancel() {
    window.location.reload();
  }

  refreshEditGrid() {
    document.getElementById("firstName")!.setAttribute('readonly', "");
    document.getElementById("lastName")!.setAttribute('readonly', "");
    document.getElementById("websiteURL")!.setAttribute('readonly', "");
    document.getElementById("phoneNumber")!.setAttribute('readonly', "");
  }

  saveEditingChanges() {
    document.getElementById("editBtn")!.className = "col-md-12 text-center";
    this.isEditing = false;
    if (this.editUserForm.valid || this.imageUrl !== null) {
      let updatedUser = UserMapper.mapUpdateUser(this.editUserForm, this.imageUrl, this.email);
      this.userService.updateUserData(updatedUser).subscribe({
        next: () => {
          this.userStorage.setFullName(`${updatedUser.firstName}`);
          this.toast.success({ summary: "Please sign out and log in again for the changes to take effect!", duration: 5000 });
          window.setTimeout(() => {
            window.location.reload();
          }, 5000);
        },
        error: (err) => {
          this.toast.error({ summary: err.error.message, duration: 5000 })
        }
      })
    }
  }

  selectFile(event: any) {
    if (!event.target.files[0] || event.target.files[0].length == 0) {
      (<HTMLInputElement>document.getElementById('uploadPicture')).value = "";
      this.toast.error({ summary: "You must select an image!", duration: 5000 });
      return;
    }

    var mimeType = event.target.files[0].type;

    if (mimeType.match(/image\/*/) == null) {
      (<HTMLInputElement>document.getElementById('uploadPicture')).value = "";
      this.toast.error({ summary: "Only images are supported", duration: 5000 });
      return;
    }

    var reader = new FileReader();
    reader.readAsDataURL(event.target.files[0]);

    reader.onload = (_event) => {
      this.imageUrl = reader.result;
    }
  }

  sendConfirmEmail() {
    this.resetPassword.sendPasswordLink(this.email).subscribe({
      next: () => {
        document.getElementById("closeBtn")?.click();
        this.toast.success({ summary: "The reset email has been sent, please check your email!", duration: 5000 });
      },
      error: (err) => {
        this.toast.error({
          summary: err.error.message,
          duration: 5000
        });
        console.log(err);
      }
    })
  }
}
