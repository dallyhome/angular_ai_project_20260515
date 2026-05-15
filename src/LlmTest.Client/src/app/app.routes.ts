import { Routes } from '@angular/router';
import { MenuPageComponent } from './menu-page/menu-page.component';
import { WeatherForecastComponent } from './weather-forecast/weather-forecast.component';
import { YahootaiwanComponent } from './yahootaiwan/yahootaiwan.component';

export const routes: Routes = [
  { path: '', component: WeatherForecastComponent },
  { path: 'yahootaiwan', component: YahootaiwanComponent },
  { path: 'menu-page', component: MenuPageComponent }
];
