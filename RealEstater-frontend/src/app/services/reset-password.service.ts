import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ResetModel } from '../models/ResetModel';

@Injectable({
  providedIn: 'root'
})
export class ResetPasswordService {
  private baseUrl: string = "https://localhost:7154/api/User";
  constructor(private http: HttpClient) { }

  sendPasswordLink(email: string){
    return this.http.post<any>(`${this.baseUrl}/sendEmail/${email}`, {});
  }

  resetPassword(resetPasswordObj: ResetModel){
    return this.http.post<any>(`${this.baseUrl}/resetPassword`, resetPasswordObj);
  }
}
