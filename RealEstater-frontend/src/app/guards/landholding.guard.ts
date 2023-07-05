import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { LandholdingService } from '../services/landholding.service';
import { Observable, map } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class LandholdingGuard implements CanActivate {

  constructor(private jwtHelper: JwtHelperService,
    private landholdingService: LandholdingService,
    private router: Router,
    private toast: NgToastService) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    let id = +route.paramMap.get('id')!;
    let token = this.jwtHelper.decodeToken(localStorage.getItem('token')!);

    return this.landholdingService.getLandholdingById(id).pipe(
      map(res => {
        if (res.userId === Number(token.id))
          return true;
        else {
          this.toast.error({ detail: "You don't have access!" });
          this.router.navigate(['/user-landholdings']);
          return false;
        }
      }));
  }
}
