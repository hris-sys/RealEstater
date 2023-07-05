import { Component, OnInit } from '@angular/core';
import { LandholdingService } from 'src/app/services/landholding.service';

@Component({
  selector: 'app-landholding-carousel',
  templateUrl: './landholding-carousel.component.html',
  styleUrls: ['./landholding-carousel.component.css']
})
export class LandholdingCarouselComponent implements OnInit {

  latestLandholdings!: any;
  landholdingPictures: any = [];

  constructor(private landholdingService: LandholdingService) { }

  ngOnInit(): void {
    this.landholdingService.getLatestLandholdings().subscribe(res => {
      this.latestLandholdings = res;
      (document.getElementById('spinner') as HTMLElement).style.display = "none";
      for (let i = 0; i < res.length; i++) {
        const element = res[i];
        this.landholdingPictures.push(element.pictures[0]);
      }
    })
  }
}
