import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { AuthService } from 'src/app/services/auth.service';
import { LandholdingService } from 'src/app/services/landholding.service';

@Component({
  selector: 'app-landholding-user-crud',
  templateUrl: './landholding-user-crud.component.html',
  styleUrls: ['./landholding-user-crud.component.css']
})
export class LandholdingUserCrudComponent implements OnInit {

  p: number = 1;
  collection: any = [];

  constructor(private router: Router,
              private toast: NgToastService,
              private landholdingService: LandholdingService,
              private auth: AuthService) { }

  ngOnInit(): void {
    const tokenPayload = this.auth.decodeToken();
    const email = tokenPayload.unique_name;
    this.landholdingService.getAllUserLandholdingsByEmail(email).subscribe(res => {
      this.collection = res;
      (document.getElementById('spinner') as HTMLElement).style.display = "none";
    })
  }

  createNewLandholding(){
    this.router.navigate(['/createLandholding']);
  }

}
