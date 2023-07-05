import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgToastService } from 'ng-angular-popup';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private auth: AuthService, private router: Router, private toast: NgToastService, private jwtHelper: JwtHelperService) { }

  canActivate(): boolean {
    let token = this.auth.isLoggedIn();
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    
    this.router.navigate(['login']);
    this.toast.error({ detail: "Please log in your account!" });
    return false;
  }
}
