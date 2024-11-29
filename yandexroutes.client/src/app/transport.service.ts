import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TransportService {
  private baseUrl = 'https://localhost:44370/Transport';

  constructor(private http: HttpClient) { }

  getVehicles() {
    return this.http.get<any[]>(`${this.baseUrl}/vehicles`);
  }

  getRoutes(vehicleId: number) {
    return this.http.get<string[][]>(`${this.baseUrl}/routes/${vehicleId}`);
  }
}

