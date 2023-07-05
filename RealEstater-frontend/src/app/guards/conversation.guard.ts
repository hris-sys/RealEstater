import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { Observable, map, take } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ConversationService } from '../services/conversation.service';

@Injectable({
  providedIn: 'root'
})
export class ConversationGuard implements CanActivate {
  constructor(
    private jwtHelper: JwtHelperService,
    private router: Router,
    private toast: NgToastService,
    private conversationService: ConversationService) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    let id = +route.paramMap.get('id')!;
    let token = this.jwtHelper.decodeToken(localStorage.getItem('token')!);

    return this.conversationService.getConversation(id).pipe(
      map((res: any) => {
        if ((res.userOne.id === Number(token.id)) || (res.userTwo.id === Number(token.id)))
          return true;
        else {
          this.toast.error({ detail: "You don't have access!" });
          this.router.navigate(['/myMessages']);
          return false;
        }
      }));
  }

}
