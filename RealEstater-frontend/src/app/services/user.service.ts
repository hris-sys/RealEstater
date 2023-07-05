import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { DisplayUserInfoModel } from '../models/DisplayUserInfoModel';
import { UpdateUserInfoModel } from '../models/UpdateUserInfoModel';
import { UserStoreService } from './user-store.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseUrl: string = "https://localhost:7154/api/User/";

  constructor(private http: HttpClient, private router: Router, private userStoreService: UserStoreService) { }

  getUserDataByEmail(userEmail: string) {
    return this.http.get<DisplayUserInfoModel>(`${this.baseUrl}getUserData/${userEmail}`);
  }

  getUserDataById(id: number) {
    return this.http.get<DisplayUserInfoModel>(`${this.baseUrl}getUserById/${id}`);
  }

  updateUserData(user: UpdateUserInfoModel) {
    return this.http.put<any>(`${this.baseUrl}updateUser`, user);
  }
}
