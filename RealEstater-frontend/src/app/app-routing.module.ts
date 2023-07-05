import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AuthGuard } from './guards/auth.guard';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { ProfileComponent } from './components/profile/profile.component';
import { LoggedInGuard } from './guards/loggedIn.guard';
import { LandholdingUserCrudComponent } from './components/landholding-user-crud/landholding-user-crud.component';
import { LandholdingCreateComponent } from './components/landholding-create/landholding-create.component';
import { LandholdingGuard } from './guards/landholding.guard';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { LandholdingEditComponent } from './components/landholding-edit/landholding-edit.component';
import { LandholdingViewComponent } from './components/landholding-view/landholding-view.component';
import { LandholdingSearchComponent } from './components/landholding-search/landholding-search.component';
import { ProfileViewComponent } from './components/profile-view/profile-view.component';
import { UserGuard } from './guards/user.guard';
import { MessagesComponent } from './components/messages/messages.component';
import { MessagesConversationComponent } from './components/messages-conversation/messages-conversation.component';
import { ConversationGuard } from './guards/conversation.guard';

const routes: Routes = [
  { path: '', component: WelcomeComponent },
  { path: 'login', component: LoginComponent, canActivate: [LoggedInGuard] },
  { path: 'signUp', component: SignUpComponent, canActivate: [LoggedInGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'resetPassword', component: ResetPasswordComponent },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'user-landholdings', component: LandholdingUserCrudComponent, canActivate: [AuthGuard] },
  { path: 'createLandholding', component: LandholdingCreateComponent, canActivate: [AuthGuard] },
  { path: 'editLandholding/:id', component: LandholdingEditComponent, canActivate: [AuthGuard, LandholdingGuard] },
  { path: 'viewLandholding/:id', component: LandholdingViewComponent, canActivate: [AuthGuard] },
  { path: 'search/:query', component: LandholdingSearchComponent, canActivate: [AuthGuard] },
  { path: 'viewProfile/:id', component: ProfileViewComponent, canActivate: [AuthGuard, UserGuard] },
  { path: 'conversation/:id', component: MessagesConversationComponent, canActivate: [AuthGuard, ConversationGuard] },
  { path: 'myMessages', component: MessagesComponent, canActivate: [AuthGuard] },

  { path: '**', pathMatch: 'full', 
  component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
