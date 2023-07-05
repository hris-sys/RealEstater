import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { LandholdingModel } from '../models/LandholdingModel';

@Injectable({
  providedIn: 'root'
})
export class LandholdingService {

  private baseUrl: string = "https://localhost:7154/api/Landholding/"
  constructor(private http: HttpClient, private router: Router) { }

  getAllConstructionStages() {
    return this.http.get(`${this.baseUrl}getAllConstructionStages`);
  }

  getAllConstructionTypes() {
    return this.http.get(`${this.baseUrl}getAllConstructionTypes`);
  }

  getAllLandholdingTypes() {
    return this.http.get(`${this.baseUrl}getAllLandholdingTypes`);
  }

  getAllFeatures() {
    return this.http.get(`${this.baseUrl}getAllFeatures`);
  }

  getAllUserLandholdingsByEmail(email: string) {
    return this.http.get(`${this.baseUrl}getAllUserLandholdingsByEmail/${email}`);
  }

  getAllUserLandholdingsById(id: number) {
    return this.http.get(`${this.baseUrl}getAllUserLandholdingsById/${id}`);
  }

  getLandholdingById(id: number) {
    return this.http.post<any>(`${this.baseUrl}getLandholdingById`, id);
  }

  updateLandholding(updateLandholding: LandholdingModel, token: string) {
    return this.http.put<any>(`${this.baseUrl}updateLandholding`, {
      CreateLandholdingDto: updateLandholding,
      Email: token,
    });
  }

  createNewLandholding(createlandholding: LandholdingModel, token: string) {
    return this.http.post<any>(`${this.baseUrl}createNewLandholding`, {
      CreateLandholdingDto: createlandholding,
      Email: token,
    });
  }

  searchLandholding(query: string) {
    return this.http.get<any>(`${this.baseUrl}searchLandholdings/${query}`);
  }

  getLatestLandholdings() {
    return this.http.get<any>(`${this.baseUrl}getLatestLandholdings`);
  }

  getLatestDiscountedLandholdings() {
    return this.http.get<any>(`${this.baseUrl}getLatestDiscounterdLadholdings`);
  }

  deleteLandholding(landholdingId: number) {
    return this.http.post<any>(`${this.baseUrl}deleteLandholding`, landholdingId);
  }

  exportToPdf(landholdingId: number) {
    return this.http.get(`${this.baseUrl}generatePdf/${landholdingId}`, { responseType: 'blob' });
  }
}
