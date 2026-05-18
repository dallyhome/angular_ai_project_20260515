import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, tap } from 'rxjs';

const AUTH_STORAGE_KEY = 'llmtest.authenticated';

interface LoginResponse {
  success: boolean;
  username: string;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = 'http://localhost:5000/api/auth/login';

  constructor(private readonly http: HttpClient) {}

  isLoggedIn(): boolean {
    return localStorage.getItem(AUTH_STORAGE_KEY) === 'true';
  }

  login(username: string, password: string): Observable<boolean> {
    return this.http.post<LoginResponse>(this.apiUrl, { username, password }).pipe(
      tap(response => {
        if (response.success) {
          localStorage.setItem(AUTH_STORAGE_KEY, 'true');
        }
      }),
      map(response => response.success)
    );
  }

  logout(): void {
    localStorage.removeItem(AUTH_STORAGE_KEY);
  }
}
