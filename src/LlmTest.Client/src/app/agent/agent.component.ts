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

  /**
   * 送出使用者輸入的自然語言問題，呼叫後端 /api/agent/ask，並把 Agent 回覆顯示在畫面上。
   */
  ask(): void {
    const message = this.message.trim();
    if (!message) {
      this.errorMessage = '請輸入要查詢的內容。';
      this.answer = '';
      return;
    }

    // 送出前清空舊結果，並切換成 loading 狀態避免重複送出。
    this.isLoading = true;
    this.errorMessage = '';
    this.answer = '';

    // 呼叫 Angular service，讓 service 負責和 ASP.NET Core API 溝通。
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
