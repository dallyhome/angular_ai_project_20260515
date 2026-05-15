import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface YahooTaiwanNewsItem {
  title: string;
  link: string;
  publishDate: string | null;
  summary: string;
}

@Injectable({
  providedIn: 'root'
})
export class YahootaiwanService {
  private readonly apiUrl = 'http://localhost:5000/api/getTaiwanYahoo';

  constructor(private readonly http: HttpClient) {}

  getTaiwanYahoo(): Observable<YahooTaiwanNewsItem[]> {
    return this.http.get<YahooTaiwanNewsItem[]>(this.apiUrl);
  }
}
