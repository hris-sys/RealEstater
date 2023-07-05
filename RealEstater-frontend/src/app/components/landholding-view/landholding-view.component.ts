import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { Observable, switchMap } from 'rxjs';
import { AvgCityPriceModel } from 'src/app/models/AvgCityPriceModel';
import { AuthService } from 'src/app/services/auth.service';
import { CitiesService } from 'src/app/services/cities.service';
import { ConversationService } from 'src/app/services/conversation.service';
import { LandholdingService } from 'src/app/services/landholding.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-landholding-view',
  templateUrl: './landholding-view.component.html',
  styleUrls: ['./landholding-view.component.css']
})
export class LandholdingViewComponent implements OnInit {

  landholdingId!: number;
  landholding!: any;

  isActive: boolean = true;
  currentUserEmail!: string;
  owner!: any;
  averagePricesInRegion!: any;

  chart: any;

  chartOptions = {
    animationEnabled: true,
    theme: "light2",
    title: {
      text: "Historical Price"
    },
    axisX: {
      valueFormatString: "MMM YYYY"
    },
    axisY: {
      title: "Price"
    },
    toolTip: {
      shared: true
    },
    legend: {
      cursor: "pointer",
      itemclick: function (e: any) {
        if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
          e.dataSeries.visible = false;
        } else {
          e.dataSeries.visible = true;
        }
        e.chart.render();
      }
    },
    data: [{
      type: "line",
      showInLegend: true,
      name: "Current Landholding Price",
      xValueFormatString: "MM YYYY",
      dataPoints: [
        { x: new Date(2023, 1, 1), y: 0 }
      ]
    },
    {
      type: "line",
      showInLegend: true,
      name: "Average prices for the region the region",
      xValueFormatString: "MM YYYY",
      dataPoints: [
        { x: new Date(2023, 1, 1), y: 0 }
      ]
    }
    ]
  };

  constructor(private route: ActivatedRoute,
    private cityService: CitiesService,
    private landholdingService: LandholdingService,
    private userService: UserService,
    private authService: AuthService,
    private toast: NgToastService,
    private conversationService: ConversationService,
    private router: Router) { }

  ngOnInit(): void {
    this.landholdingId = Number(this.route.snapshot.paramMap.get('id'))!;
    this.currentUserEmail = this.authService.getEmailFromToken();

    this.getLandholding().pipe(
      switchMap((landholding) => {

        if (!landholding)
          this.router.navigate(['/err']);

        this.landholding = landholding;
        this.isActive = landholding.isActive;
        this.chartOptions.data[0].dataPoints.pop();
        this.chartOptions.data[1].dataPoints.pop();
        for (let i = 0; i < this.landholding.historyPrices.length; i++) {
          const element = this.landholding.historyPrices[i];
          this.chartOptions.data[0].dataPoints.push({ x: new Date(element.startDate), y: Number(element.price.toFixed(2)) });
        }
        return this.getAvgOfPricesInRegion(landholding);
      }),
      switchMap((res) => {
        for (let i = 0; i < res.length; i++) {
          const element = res[i].price;
          const time = res[i].startDate;
          this.chartOptions.data[1].dataPoints.push({ x: new Date(time), y: element.toFixed(0) });
        }
        return this.userService.getUserDataById(this.landholding.userId);
      })
    ).subscribe((res) => {
      this.owner = res;
      (document.getElementById('spinner') as HTMLElement).style.display = "none";
      (document.getElementById('card') as HTMLElement).style.display = "";
    });
  }

  getLandholding(): Observable<any> {
    return this.landholdingService.getLandholdingById(this.landholdingId);
  }

  getAvgOfPricesInRegion(landholding: any): Observable<any> {
    let input = new AvgCityPriceModel();

    input.city = landholding.city;
    input.history = [];

    for (let i = 0; i < landholding.historyPrices.length; i++) {
      const element = landholding.historyPrices[i].startDate;
      input.history.push(element);
    }

    return this.cityService.getAveragePriceForRegion(input);
  }

  sendMessage() {
    var message = (document.getElementById('message') as HTMLInputElement).value;
    if (message.length <= 10) {
      this.toast.warning({ summary: "Please enter more than 10 characters to send a message!", duration: 5000 });
    }
    else {
      this.conversationService.sendMessage(this.currentUserEmail, this.owner.email, message).subscribe(res => {
        this.toast.success({ summary: "Message sent succesfully, please check your inbox!", duration: 5000 });
        message = "";
        document.getElementById("closeModal")!.click();
      });
    }
  }

  exportToPdf() {
    this.toast.info({summary: "Please wait 2-5 seconds!", duration: 5000});
    this.landholdingService.exportToPdf(this.landholdingId).subscribe((res: any) => {
      const blobUrl = URL.createObjectURL(res);

      const link = document.createElement('a');
      link.href = blobUrl;
      link.download = `${this.landholding.title}-exported.pdf`;
      link.click();

      URL.revokeObjectURL(blobUrl);
    });
  }
}
