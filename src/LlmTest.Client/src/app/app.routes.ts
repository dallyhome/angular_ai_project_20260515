import { Routes } from '@angular/router';
import { authGuard } from './auth/auth.guard';
import { LoginComponent } from './login/login.component';
import { MenuPageComponent } from './menu-page/menu-page.component';
import { WeatherForecastComponent } from './weather-forecast/weather-forecast.component';
import { YahootaiwanComponent } from './yahootaiwan/yahootaiwan.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', component: WeatherForecastComponent, canActivate: [authGuard] },
  { path: 'yahootaiwan', component: YahootaiwanComponent, canActivate: [authGuard] },
  { path: 'menu-page', component: MenuPageComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '' }
];
