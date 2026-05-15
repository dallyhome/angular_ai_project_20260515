import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { WeatherForecast, WeatherforcaseService } from '../weatherforcase/weatherforcase.service';

@Component({
  selector: 'app-weather-forecast',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './weather-forecast.component.html',
  styleUrl: './weather-forecast.component.css'
})
export class WeatherForecastComponent implements OnInit {
  forecasts: WeatherForecast[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private readonly weatherforcaseService: WeatherforcaseService) {}

  ngOnInit(): void {
    this.loadWeatherForecast();
  }

  loadWeatherForecast(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.weatherforcaseService.getWeatherForecast()
      .subscribe({
        next: forecasts => {
          this.forecasts = forecasts;
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage = 'Unable to load weather forecast data. Please make sure Web API is running.';
          this.isLoading = false;
        }
      });
  }
}
