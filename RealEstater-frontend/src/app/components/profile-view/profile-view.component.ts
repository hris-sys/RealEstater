import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { switchMap } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { ConversationService } from 'src/app/services/conversation.service';
import { LandholdingService } from 'src/app/services/landholding.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile-view',
  templateUrl: './profile-view.component.html',
  styleUrls: ['./profile-view.component.css']
})
export class ProfileViewComponent implements OnInit {

  p: number = 1;
  collection: any = [];
  user!: any;
  loggedInUser!: any;

  constructor(private userService: UserService,
    private route: ActivatedRoute,
    private landholdingService: LandholdingService,
    private conversationService: ConversationService,
    private authService: AuthService,
    private toast: NgToastService) { }

  ngOnInit(): void {
    this.loggedInUser = this.authService.getEmailFromToken();

    this.userService.getUserDataById(Number(this.route.snapshot.paramMap.get('id'))!).pipe(
      switchMap((res) => {
        this.user = res;
        return this.landholdingService.getAllUserLandholdingsById(this.user.id);
      })
    ).subscribe((res) => {
      this.collection = res;
      (document.getElementById('spinner') as HTMLElement).style.display = "none";
    });
  }

  sendMessage() {
    var message = (document.getElementById('message') as HTMLInputElement).value;
    if (message.length <= 10) {
      this.toast.warning({ summary: "Please enter more than 10 characters to send a message!", duration: 5000 });
    }
    else {
      this.conversationService.sendMessage(this.loggedInUser, this.user.email, message).subscribe(res => {
        this.toast.success({ summary: "Message sent succesfully, please check your inbox!", duration: 5000 });
        message = "";
        document.getElementById("closeModal")!.click();
      });
    }
  }

}
