import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { LoginModel } from '../../models/login.model';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnDestroy {
  public loginModel = new LoginModel();
  public hasError = false;

  private destroy$: Subject<boolean> = new Subject();

  constructor(private authService: AuthService, private router: Router) {}

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  public onLogin(): void {
    this.authService
      .login(this.loginModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe((response => {
        this.hasError = !response.succeeded;
      }));
  }

  public onNavigateToRegister(): void {
    this.router.navigate(['account', 'register']);
  }
}
