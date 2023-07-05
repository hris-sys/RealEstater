import { HttpClient } from '@angular/common/http';
import { Injectable, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ConversationService {

  private baseUrl: string = "https://localhost:7154/api/Conversation/";

  constructor(private http: HttpClient, private router: Router) { }

  getConversation(id: number) {
    return this.http.get(`${this.baseUrl}getConversation/${id}`);
  }

  sendMessage(userFrom: string, userTo: string, message: string) {
    return this.http.post<any>(`${this.baseUrl}sendMessage`, {
      UserFrom: userFrom,
      UserTo: userTo,
      Message: message
    });
  }

  getAllUserMessages(email: string) {
    return this.http.get(`${this.baseUrl}getAllUserConversations/${email}`);
  }
}
