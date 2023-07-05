import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BrowserModule } from '@angular/platform-browser';
import { NgToastModule } from 'ng-angular-popup'
import { NgxPaginationModule } from 'ngx-pagination';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { ProfileComponent } from './components/profile/profile.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FooterComponent } from './components/footer/footer.component';
import { JWT_OPTIONS, JwtHelperService } from '@auth0/angular-jwt';
import { LandholdingCarouselComponent } from './components/landholding-carousel/landholding-carousel.component';
import { LandholdingUserCrudComponent } from './components/landholding-user-crud/landholding-user-crud.component';
import { LandholdingCreateComponent } from './components/landholding-create/landholding-create.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { LandholdingEditComponent } from './components/landholding-edit/landholding-edit.component';
import { LandholdingViewComponent } from './components/landholding-view/landholding-view.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import * as CanvasJSAngularChart from '../assets/canvasjs.angular.component';
import { LandholdingSearchComponent } from './components/landholding-search/landholding-search.component';
import { ProfileViewComponent } from './components/profile-view/profile-view.component';
import { MessagesComponent } from './components/messages/messages.component';
import { MessagesConversationComponent } from './components/messages-conversation/messages-conversation.component';
var CanvasJSChart = CanvasJSAngularChart.CanvasJSChart;

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignUpComponent,
    DashboardComponent,
    ResetPasswordComponent,
    WelcomeComponent,
    ProfileComponent,
    NavbarComponent,
    FooterComponent,
    LandholdingCarouselComponent,
    LandholdingUserCrudComponent,
    LandholdingCreateComponent,
    PageNotFoundComponent,
    LandholdingEditComponent,
    LandholdingViewComponent,
    CanvasJSChart,
    LandholdingSearchComponent,
    ProfileViewComponent,
    MessagesComponent,
    MessagesConversationComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgToastModule,
    FormsModule,
    NgxPaginationModule,
    FontAwesomeModule,
    BrowserAnimationsModule,
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi: true
  },
  { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
    JwtHelperService],
  bootstrap: [AppComponent],
})
export class AppModule { }
