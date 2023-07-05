import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserStoreService {

  private fullName$ = new BehaviorSubject<string>("");
  constructor() { }

  public getFullName() {
    return this.fullName$.asObservable();
  }

  public setFullName(fullName: string) {
    this.fullName$.next(fullName);
  }
}
