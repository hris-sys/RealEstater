import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
  HttpHeaders
} from '@angular/common/http';
import { catchError, Observable, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { NgToastService } from 'ng-angular-popup';
import { Router } from '@angular/router';
import { TokenApiModel } from '../models/TokenApiModel';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private auth: AuthService, private toast: NgToastService, private router: Router) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const myToken = this.auth.getToken();

    if (myToken) {
      request = request.clone({
        setHeaders: { Authorization: `Bearer: ${myToken}` }
      });
    }
    return next.handle(request).pipe(
      catchError((err: any) => {
        if (err instanceof HttpErrorResponse) {
          if (err.status === 401) {
            return this.handleUnAuthorizedError(request, next);
          }
        }
        return next.handle(request);
      })
    );
  }

  handleUnAuthorizedError(request: HttpRequest<any>, next: HttpHandler) {
    let tokenApiModel = new TokenApiModel();
    tokenApiModel.accessToken = this.auth.getToken();
    tokenApiModel.refreshToken = this.auth.getRefreshToken();

    return this.auth.renewToken(tokenApiModel)
      .pipe(switchMap((data: TokenApiModel) => {
        this.auth.storeRefreshToken(data.refreshToken);
        this.auth.storeToken(data.accessToken);

        let headers = new HttpHeaders({
          'Authorization': 'Bearer ' + data.accessToken
        });

        request = request.clone({
          headers: headers
        });
        return next.handle(request);
      }),
        catchError((err) => {
          return throwError(() => {
            this.toast.warning({ summary: "Your sessions has expired please log in again!" });
            this.router.navigate(["login"]);
          })
        }))
  }
}
