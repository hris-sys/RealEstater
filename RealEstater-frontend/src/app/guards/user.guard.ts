import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, UrlTree } from '@angular/router';
import { Observable, map } from 'rxjs';
import { UserService } from '../services/user.service';
import { NgToastService } from 'ng-angular-popup';

@Injectable({
  providedIn: 'root'
})
export class UserGuard implements CanActivate {

  constructor(private userService: UserService,
              private toast: NgToastService,
              private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot,) : Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    let id = +route.paramMap.get('id')!;
    
    return this.userService.getUserDataById(id).pipe(
      map(res => {
        if (res)
          return true;
        else {
          this.router.navigate(['/**']);
          return false;
        }
      }));
  }
}
