import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import ValidateForm from '../../helpers/ValidateForm';
import { ResetModel } from '../../models/ResetModel';
import { ResetPasswordService } from '../../services/reset-password.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {

  userEmail!: string;
  resetPasswordForm!: FormGroup;
  emailToken!: string;

  confirmedPassword!: string;
  newPassword!: string;

  resetPasswordObj: ResetModel = new ResetModel();

  constructor(private route: Router,
    private toast: NgToastService,
    private activatedRouter: ActivatedRoute,
    private resetService: ResetPasswordService) { }

  ngOnInit(): void {
    this.resetPasswordForm = new FormGroup({
      newPassword: new FormControl('', [Validators.required, Validators.pattern(/^(?=.*\d)(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z]).{8,}$/)]),
      confirmNewPassword: new FormControl('', [Validators.required, this.passwordMatchValidator.bind(this)])
    });

    this.activatedRouter.queryParams.subscribe(res => {
      this.userEmail = res['email'];
      let uriToken = res['code'];
      this.emailToken = uriToken.replace(/ /g, '+');
    });
  }

  passwordMatchValidator(control: FormControl): { [key: string]: boolean } | null {
    const passwordControl = control.root.get('newPassword');
    if (passwordControl) {
      const passwordValue = passwordControl.value;
      if (control.value !== passwordValue) {
        return { 'passwordMismatch': true };
      }
    }
    return {};
  }

  resetPassword() {
    if (this.resetPasswordForm.valid) {
      this.resetPasswordObj.confirmPassword = this.resetPasswordForm.value.confirmNewPassword;
      this.resetPasswordObj.email = this.userEmail;
      this.resetPasswordObj.emailToken = this.emailToken;
      this.resetPasswordObj.newPassword = this.resetPasswordForm.value.newPassword;

      this.resetService.resetPassword(this.resetPasswordObj).subscribe({
        next: () => {
          this.toast.success({ summary: "Password changed successfully! You may close the window.", duration: 5000 });
          (document.getElementById('resetButtonId') as HTMLButtonElement).disabled = true;
        },
        error: (err) => {
          this.toast.error({summary: err.error.message, duration: 5000});
          console.log(err);
        }
      }
      );
    }
    else {
      ValidateForm.validateAllFormFields(this.resetPasswordForm);
      this.toast.error({ summary: "Please make sure all the fields are entered!", duration: 5000 })
    }
  }

}
