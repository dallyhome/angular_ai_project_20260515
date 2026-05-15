import { CommonModule, DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { YahooTaiwanNewsItem, YahootaiwanService } from './yahootaiwan.service';

@Component({
  selector: 'app-yahootaiwan',
  standalone: true,
  imports: [CommonModule, DatePipe],
  templateUrl: './yahootaiwan.component.html',
  styleUrl: './yahootaiwan.component.css'
})
export class YahootaiwanComponent implements OnInit {
  news: YahooTaiwanNewsItem[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private readonly yahootaiwanService: YahootaiwanService) {}

  ngOnInit(): void {
    this.loadNews();
  }

  loadNews(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.yahootaiwanService.getTaiwanYahoo()
      .subscribe({
        next: news => {
          this.news = news;
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage = 'Unable to load Yahoo Taiwan news. Please make sure Web API is running.';
          this.isLoading = false;
        }
      });
  }
}
