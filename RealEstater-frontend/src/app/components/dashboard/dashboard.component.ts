import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { LandholdingService } from 'src/app/services/landholding.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  private baseUrl: string = "https://localhost:7154/api/User/"
  public fullName!: string;
  public role!: string;
  public users!: any;
  
  latestDiscountedLandholdings!: any;

  constructor(private auth: AuthService,
    private landholdingService: LandholdingService) { }

  ngOnInit(): void {
    this.fullName = this.auth.getFullNameFromToken();

    this.landholdingService.getLatestDiscountedLandholdings().
      subscribe(res => {
        this.latestDiscountedLandholdings = res;
      });
  }

}
