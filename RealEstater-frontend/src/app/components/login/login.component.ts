import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import ValidateForm from 'src/app/helpers/ValidateForm';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';
import { ResetPasswordService } from '../../services/reset-password.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;
  sendEmailForm!: FormGroup;
  isValidEmail!: boolean;

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });
    this.sendEmailForm = this.fb.group({
      forgotPasswordEmail: new FormControl('', [Validators.required, Validators.email])
    })
  }

  constructor(private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private toast: NgToastService,
    private userStore: UserStoreService,
    private resetPassword: ResetPasswordService) { }

  onLogin() {
    if (this.loginForm.valid) {
      this.auth.login(this.loginForm.value)
        .subscribe({
          next: (res) => {
            this.toast.success({ summary: "Successful logged in!", duration: 5000 });
            this.auth.storeToken(res.accessToken);
            this.auth.storeRefreshToken(res.refreshToken);
            const tokenPayload = this.auth.decodeToken();
            this.userStore.setFullName(tokenPayload.unique_name);
            this.loginForm.reset();
            this.router.navigate(['dashboard']);
          },
          error: (err) => {
            this.toast.error({ summary: err.error.message, duration: 5000 });
          }
        })
    }
    else {
      ValidateForm.validateAllFormFields(this.loginForm);
      this.toast.error({summary: "Please make sure all the fields are entered!", duration: 5000})
    }
  }

  sendConfirmEmail() {
    if (this.sendEmailForm.valid) {
      this.resetPassword.sendPasswordLink(this.sendEmailForm.controls['forgotPasswordEmail'].value).subscribe({
        next: (res) => {
          this.sendEmailForm.reset();
          document.getElementById("closeBtn")?.click();
          this.toast.success({ summary: "The reset email has been sent, please check your email!", duration: 5000 });
        },
        error: (err) => {
          this.toast.error({
            summary: err.error.message,
            duration: 5000
          });
        }
      })
    }
  }
}
