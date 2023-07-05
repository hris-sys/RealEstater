import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AvgCityPriceModel } from '../models/AvgCityPriceModel';

@Injectable({
  providedIn: 'root'
})
export class CitiesService {

  private baseUrl: string = "https://localhost:7154/api/City/"
  constructor(private http: HttpClient, private router: Router) { }

  getAllCities(){
    return this.http.get(`${this.baseUrl}getAllCities`);
  }

  getAveragePriceForRegion(input: AvgCityPriceModel){
    return this.http.post<any>(`${this.baseUrl}getAveragePriceForRegion`, input);
  }

  getAllAveragePriceForRegion(input: AvgCityPriceModel){
    return this.http.post<any>(`${this.baseUrl}getAllAveragePriceForRegion`, input);
  }

}
