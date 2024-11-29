import { Component } from '@angular/core';
import { TransportService } from './transport.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  vehicles: any[] = [];
  routes: string[] = [];

  constructor(private transportService: TransportService) { }

  loadVehicles() {
    this.transportService.getVehicles().subscribe({
      next: (data) => (this.vehicles = data),
      error: (err) => console.error('Ошибка загрузки транспорта:', err),
    });
  }

  loadRoutes(vehicleId: number) {
    this.transportService.getRoutes(vehicleId).subscribe({
      next: (data) => (this.routes = data[0]),
      error: (err) => console.error('Ошибка загрузки маршрута:', err),
    });
  }
}
