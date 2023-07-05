import { AfterViewInit, Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable, switchMap } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { ConversationService } from 'src/app/services/conversation.service';
import { MessagesComponent } from '../messages/messages.component';

@Component({
  selector: 'app-messages-conversation',
  templateUrl: './messages-conversation.component.html',
  styleUrls: ['./messages-conversation.component.css']
})
export class MessagesConversationComponent {

  @ViewChild('scrollContainer', { static: false }) scrollContainer!: ElementRef;

  @Output()
  refreshParentGrid: EventEmitter<any> = new EventEmitter();

  selectedConversation!: any;
  conversationData: any = [];
  token!: any;
  currentUserEmail!: any;
  currentUserSendToEmail!: any;

  constructor(private conversationService: ConversationService, private jwtHelper: JwtHelperService, private authService: AuthService) { }

  initialize(selectedConversation: any) {
    this.selectedConversation = selectedConversation;
    this.currentUserEmail = this.authService.getEmailFromToken();
    this.token = this.jwtHelper.decodeToken(localStorage.getItem('token')!);
    this.currentUserSendToEmail = selectedConversation.userOneEmail == this.currentUserEmail ? selectedConversation.userTwoEmail : selectedConversation.userOneEmail;
    this.refreshConversationGrid().subscribe((res: any) => {
      this.conversationData = res.replies;
      (document.getElementById('spinner') as HTMLElement).style.display = "none";
    });
    this.scrollToBottom();
  }

  refreshConversationGrid(): Observable<any> {
    return this.conversationService.getConversation(this.selectedConversation.conversationId);
  }

  sendMessage() {
    var message = (document.getElementById('textAreaSend') as HTMLInputElement).value;
    if (message.trim() === "" || message === null) return;

    this.conversationService.sendMessage(this.currentUserEmail, this.currentUserSendToEmail, message).pipe(
      switchMap(() => {
        return this.refreshConversationGrid();
      })
    ).subscribe(res => {
      this.conversationData = res.replies;
      (document.getElementById('textAreaSend') as HTMLInputElement).value = "";
      this.scrollToBottom();
    });
    
    this.refreshParentGrid.emit();
  }

  scrollToBottom() {
    const container = this.scrollContainer.nativeElement;
    setTimeout(() => {
      container.scrollTop = container.scrollHeight;
    }, 100);
  }
}
