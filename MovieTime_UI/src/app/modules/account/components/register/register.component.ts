import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { RegisterModel } from '../../models/register.model';

@Component({
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnDestroy {
  public registerModel = new RegisterModel();
  public hasError = false;

  private destroy$: Subject<boolean> = new Subject();

  constructor(private authService: AuthService, private router: Router) {}

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  public onRegister(): void {
    this.authService
      .register(this.registerModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.hasError = !response.succeeded;
          if (!response.succeeded) return;
          this.router.navigate(['account', 'login']);
        },
        error: (error) => {}
      });
  }

  public onBackToLogin(): void {
    this.router.navigate(['account', 'login']);
  }
}
