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

  ask(message: string): Observable<AgentAskResponse> {
    return this.http.post<AgentAskResponse>(this.apiUrl, { message });
  }
}
