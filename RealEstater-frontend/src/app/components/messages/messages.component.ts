import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Observable, switchMap } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { ConversationService } from 'src/app/services/conversation.service';
import { UserService } from 'src/app/services/user.service';
import { MessagesConversationComponent } from '../messages-conversation/messages-conversation.component';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  @ViewChild(MessagesConversationComponent) childComponent!: MessagesConversationComponent;
  collection: any = [];
  currentUser!: any;
  currentUserEmail!: string;
  currentUserFullName!: string;
  clickedOnSomeone!: boolean;

  constructor(private conversationService: ConversationService,
    private authService: AuthService,
    private userService: UserService) { }

  ngOnInit(): void {
    this.currentUserEmail = this.authService.getEmailFromToken();
    this.userService.getUserDataByEmail(this.currentUserEmail).pipe(
      switchMap(res => {
        this.currentUser = res;
        this.currentUserFullName = `${res.firstName} ${res.lastName}`;
        return this.refreshConversationGrid();
      })
      ).subscribe(x => {
        this.collection = x;
        (document.getElementById('spinner') as HTMLElement).style.display = "none";
    });
    this.clickedOnSomeone = false;
  }

  refreshConversationGrid(): Observable<any> {
    return this.conversationService.getAllUserMessages(this.currentUserEmail);
  }

  selectConversationForChatting(conversation: any) {
    setTimeout(() => {
      this.childComponent.initialize(conversation);
    }, 100)

    setTimeout(() => {
      this.childComponent.initialize(conversation);
    }, 100);
    this.clickedOnSomeone = true;
  }

  async handleRefreshFromChild() {
    await this.promisedRefresh();
  }

  promisedRefresh(): Promise<any> {
    return new Promise(() => {
      this.refreshConversationGrid().subscribe(res => this.collection = res);
    });
  }
}
