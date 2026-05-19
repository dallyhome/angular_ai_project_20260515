import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AgentService } from './agent.service';

@Component({
  selector: 'app-agent',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './agent.component.html',
  styleUrl: './agent.component.css'
})
export class AgentComponent {
  message = '幫我查訂單 A001';
  answer = '';
  isLoading = false;
  errorMessage = '';

  constructor(private readonly agentService: AgentService) {}

  ask(): void {
    const message = this.message.trim();
    if (!message) {
      this.errorMessage = '請輸入要查詢的內容。';
      this.answer = '';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.answer = '';

    this.agentService.ask(message)
      .subscribe({
        next: response => {
          this.answer = response.answer;
          this.isLoading = false;
        },
        error: error => {
          this.errorMessage = error?.error?.answer
            ?? '無法取得 Agent 回覆，請確認 Web API 已啟動。';
          this.isLoading = false;
        }
      });
  }
}
