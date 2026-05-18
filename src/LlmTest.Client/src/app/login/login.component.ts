import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  username = 'admin';
  password = 'admin';
  errorMessage = '';
  isSubmitting = false;

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  login(): void {
    this.errorMessage = '';
    this.isSubmitting = true;

    this.authService.login(this.username, this.password)
      .subscribe({
        next: isLoggedIn => {
          this.isSubmitting = false;

          if (isLoggedIn) {
            this.router.navigateByUrl('/');
            return;
          }

          this.errorMessage = '帳號或密碼錯誤';
        },
        error: () => {
          this.isSubmitting = false;
          this.errorMessage = '帳號或密碼錯誤';
        }
      });
  }
}
