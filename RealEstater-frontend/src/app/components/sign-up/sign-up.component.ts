import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import Common from 'src/app/helpers/Common';
import ValidateForm from 'src/app/helpers/ValidateForm';
import { AuthService } from 'src/app/services/auth.service';
import UserMapper from '../../helpers/UserMapper';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {

  signUpForm!: FormGroup;
  password!: string;
  confirmedPassword!: string;
  passwordsMatch: boolean = false;
  showPasswordOne: boolean = false;
  showPasswordTwo: boolean = false;

  ngOnInit(): void {
    this.signUpForm = new FormGroup({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      websiteURL: new FormControl('', [Validators.required, Validators.pattern(Common.regexForWebsite)]),
      phoneNumber: new FormControl('', [Validators.required, Validators.pattern(Common.regexForPhone)]),
      password: new FormControl('', [Validators.required, Validators.pattern(Common.regexForPassword)]),
      confirmPassword: new FormControl('', [Validators.required, this.passwordMatchValidator.bind(this)])
    });
  }

  constructor(private fb: FormBuilder, 
              private auth: AuthService, 
              private router: Router, 
              private toast: NgToastService) { }

  onSignup() {
    if (this.signUpForm.valid) {
      let registerUser = UserMapper.createUser(this.signUpForm);
      this.auth.signUp(registerUser)
        .subscribe({
          next: (res) => {
            this.toast.success({summary: res.message, duration: 5000});
            this.signUpForm.reset();
            this.router.navigate(['login']);
          },
          error: (err) => {
            this.toast.error({summary: err.error.message, duration: 5000});
          }
        })
    }
    else {
      ValidateForm.validateAllFormFields(this.signUpForm);
      this.toast.error({summary: "Please make sure all the fields are entered!", duration: 5000});
    }
  }

  passwordMatchValidator(control: FormControl): { [key: string]: boolean } | null {
    const passwordControl = control.root.get('password');
    if (passwordControl) {
      const passwordValue = passwordControl.value;
      if (control.value !== passwordValue) {
        return { 'passwordMismatch': true };
      }
    }
    return {};
  }

  togglePasswordOne(): void {
    this.showPasswordOne = !this.showPasswordOne;
  }

  togglePasswordTwo(): void {
    this.showPasswordTwo = !this.showPasswordTwo;
  }
}
