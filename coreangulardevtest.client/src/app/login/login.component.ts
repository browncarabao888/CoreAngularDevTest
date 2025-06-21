import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountsService } from '../accounts/accounts.service';
import { MatSnackBar, MatSnackBarRef, TextOnlySnackBar } from '@angular/material/snack-bar';
import { EncryptionService } from './../service/encryption.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;
  hidePassword = true;
  encrypted: string;
  decrypted: string;
  private snkbarRef: MatSnackBarRef<TextOnlySnackBar> | null = null;

  constructor(
    private router: Router,
    private accountsService: AccountsService,
    private fb: FormBuilder,
    private snkbar: MatSnackBar,
    private encryptionService: EncryptionService
  ) {
    const original = 'MySuperSecretPassword';
    this.encrypted = this.encryptionService.encrypt(original);
    this.decrypted = this.encryptionService.decrypt(this.encrypted);

    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }

  togglePasswordVisibility() {
    this.hidePassword = !this.hidePassword;
  }

  onLogin() {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      const form = this.loginForm;
      let msg = '';
      this.accountsService.login(email, password).subscribe({
        next: response => {
          this.router.navigate(['searchpage']);
        },
        error: err => {
          let msg = 'An unexpected error occurred.';

          if (err.error && err.error.errorCode) {
            msg = `Error ${err.error.errorCode}: ${err.error.errorMessage}`;
          } else if (err.status === 400) {
            msg = 'Invalid input or email already in use.';
          } else if (err.status === 409) {
            msg = 'Bad Request, check you inputs';
          }

          this.snkbar.open(msg, 'Close', {
            duration: 3000,
            panelClass: ['snackbar-error']
          });

        }
      });

    }
  }



}
