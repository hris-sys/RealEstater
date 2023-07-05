import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TokenApiModel } from '../models/TokenApiModel';
import { UserModel } from '../models/UserModel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl: string = "https://localhost:7154/api/User/"
  private userPayload: any;

  constructor(private http: HttpClient, private router: Router) {
  }

  signUp(userObj: UserModel) {
    return this.http.post<any>(`${this.baseUrl}register`, userObj);
  }

  login(loginObj: any) {
    return this.http.post<any>(`${this.baseUrl}authenticate`, loginObj);
  }

  storeToken(tokenValue: string) {
    localStorage.setItem('token', tokenValue);
  }

  storeRefreshToken(tokenValue: string) {
    localStorage.setItem('refreshToken', tokenValue);
  }

  getToken() {
    return localStorage.getItem('token') || '{}';
  }

  getRefreshToken() {
    return localStorage.getItem('refreshToken') || '{}';
  }

  isLoggedIn() {
    return localStorage.getItem('token');
  }

  signOut() {
    localStorage.clear();
    this.router.navigate(['login']);
  }

  decodeToken() {
    const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;
    return jwtHelper.decodeToken(token);
  }

  getFullNameFromToken() {
    this.userPayload = this.decodeToken();
    if (this.userPayload)
      return this.userPayload.given_name;
  }

  getRoleFromToken() {
    this.userPayload = this.decodeToken();
    if (this.userPayload)
      return this.userPayload.role;
  }

  getEmailFromToken() {
    this.userPayload = this.decodeToken();
    if (this.userPayload)
      return this.userPayload.unique_name;
  }

  renewToken(tokenApi: TokenApiModel) {
    return this.http.post<any>(`${this.baseUrl}refresh`, tokenApi);
  }
}
