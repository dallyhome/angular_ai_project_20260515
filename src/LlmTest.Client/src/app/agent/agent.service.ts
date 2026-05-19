import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface AgentAskRequest {
  message: string;
}

export interface AgentAskResponse {
  answer: string;
}

@Injectable({
  providedIn: 'root'
})
export class AgentService {
  private readonly apiUrl = 'http://localhost:5000/api/agent/ask';

  constructor(private readonly http: HttpClient) {}

  /**
   * 呼叫 ASP.NET Core Agent API，將使用者輸入送到後端解析並取得回答。
   */
  ask(message: string): Observable<AgentAskResponse> {
    return this.http.post<AgentAskResponse>(this.apiUrl, { message });
  }
}
