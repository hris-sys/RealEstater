import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { CitiesService } from 'src/app/services/cities.service';
import { LandholdingService } from 'src/app/services/landholding.service';

@Component({
  selector: 'app-landholding-search',
  templateUrl: './landholding-search.component.html',
  styleUrls: ['./landholding-search.component.css']
})
export class LandholdingSearchComponent implements OnInit {
  query!: string;

  p: number = 1;
  collection: any = [];

  cities: any = [];
  constructionStages: any = [];
  constructionTypes: any = [];
  landholdingTypes: any = [];

  lastSelectedCity: string = '';
  lastSelectedStage: string = '';
  lastSelectedType: string = '';
  lastSelectedLandholdingType: string = '';
  lastSelectedPrice: string = '';
  lastSelectedDate: string = '';

  constructor(
    private landholdingService: LandholdingService,
    private route: ActivatedRoute,
    private cityService: CitiesService,) { }

  ngOnInit(): void {
    this.query = this.route.snapshot.paramMap.get('query')!;

    this.landholdingService.searchLandholding(this.query).pipe(
      switchMap(res => {
        this.collection = res;
        return this.cityService.getAllCities();
      }),
      switchMap((res) => {
        this.cities = res;
        return this.landholdingService.getAllConstructionStages();
      }),
      switchMap((res) => {
        this.constructionStages = res;
        return this.landholdingService.getAllConstructionTypes();
      }),
      switchMap((res) => {
        this.constructionTypes = res;
        return this.landholdingService.getAllLandholdingTypes();
      })).subscribe(res => {
        this.landholdingTypes = res;
        (document.getElementById('spinner') as HTMLElement).style.display = "none";

      });
  }

  filterCity(selectedValue: string) {
    if (this.lastSelectedCity === selectedValue)
      return;

    this.lastSelectedCity = selectedValue;
    this.sendFilterQuery();
  }

  filterStage(selectedValue: string) {
    if (this.lastSelectedStage === selectedValue)
      return;

    this.lastSelectedStage = selectedValue;
    this.sendFilterQuery();
  }

  filterConstructionType(selectedValue: string) {
    if (this.lastSelectedType === selectedValue)
      return;

    this.lastSelectedType = selectedValue;
    this.sendFilterQuery();
  }

  filterLandholdingType(selectedValue: string) {
    if (this.lastSelectedLandholdingType === selectedValue)
      return;

    this.lastSelectedLandholdingType = selectedValue;
    this.sendFilterQuery();
  }

  filterPrice(selectedValue: string) {
    if (selectedValue === "lowToHigh")
      this.collection = this.collection.sort((a: any, b: any) => a.price - b.price);
    else
      this.collection = this.collection.sort((a: any, b: any) => b.price - a.price);
  }

  filterDate(selectedValue: string) {
    if (selectedValue === "oldToRecent")
      this.collection = this.collection.sort((a: any, b: any) => new Date(b.createdAt).getDay() - new Date(a.createdAt).getDay());
    else
      this.collection = this.collection.sort((a: any, b: any) => new Date(a.createdAt).getDay() - new Date(b.createdAt).getDay());
  }

  sendFilterQuery() {
    this.landholdingService.searchLandholding(`querySearch=${this.query};city=${this.lastSelectedCity};holdingType=${this.lastSelectedLandholdingType};stage=${this.lastSelectedStage};materialType=${this.lastSelectedType}`)
      .subscribe(res => {
        this.collection = res;
      });
  }

  clearFilters() {
    window.location.reload();
  }
}
