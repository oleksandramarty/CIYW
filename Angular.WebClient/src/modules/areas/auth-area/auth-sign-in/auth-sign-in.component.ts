import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Subject } from "rxjs";
import { MatSnackBar } from "@angular/material/snack-bar";
import { LocalizationService } from "../../../../core/services/localization.service";
import { AuthService } from "../../../../core/services/auth.service";

@Component({
  selector: 'app-auth-sign-in',
  templateUrl: './auth-sign-in.component.html',
  styleUrl: '../auth-area/auth-area.component.scss',
})
export class AuthSignInComponent implements OnInit {
  protected ngUnsubscribe: Subject<void> = new Subject<void>();
  loginForm: FormGroup | undefined;

  constructor(
    private readonly snackBar: MatSnackBar,
    private fb: FormBuilder,
    private readonly localizationService: LocalizationService,
    private readonly authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      login: ['', [Validators.required]],
      password: ['', [Validators.required]],
      rememberMe: [false]
    });
  }

  public login(): void {
    if (!this.loginForm!.valid) {
      Object.keys(this.loginForm!.controls).forEach(key => {
        const control = this.loginForm!.get(key);
        if (control) {
          control.markAsTouched();
        }
      });
      this.snackBar.open(this.localizationService?.getTranslation('ERROR.FORM_VALIDATION') ?? 'ERROR', 'Close', { duration: 3000 });
      return;
    }

    this.authService.login(
      this.loginForm!.value.login,
      this.loginForm!.value.password,
      this.loginForm!.value.rememberMe,
      this.ngUnsubscribe);
  }
}
