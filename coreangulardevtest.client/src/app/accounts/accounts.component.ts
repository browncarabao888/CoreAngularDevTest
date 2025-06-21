import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { AccountsService } from './accounts.service';
import { MatSnackBar, MatSnackBarRef, TextOnlySnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.css']
})
export class AccountsComponent {
  createAccountForm!: FormGroup<{
    firstName: FormControl<string | null>;
    lastName: FormControl<string | null>;
    email: FormControl<string | null>;
    password: FormControl<string | null>;
    confirmPassword: FormControl<string | null>;
    acceptTerms: FormControl<boolean>;
  }>;

  hidePassword = true;
  hideConfirmPassword = true;

  constructor(
    private accountsService: AccountsService,
    iconRegistry: MatIconRegistry,
    sanitizer: DomSanitizer,
    private fb: FormBuilder,
    private snkbar: MatSnackBar
  ) {
    iconRegistry.addSvgIcon(
      'google-icon',
      sanitizer.bypassSecurityTrustResourceUrl('assets/icons/google.svg')
    );
    iconRegistry.addSvgIcon(
      'facebook-icon',
      sanitizer.bypassSecurityTrustResourceUrl('assets/icons/facebook.svg')
    );
    iconRegistry.addSvgIcon(
      'twitter-icon',
      sanitizer.bypassSecurityTrustResourceUrl('assets/icons/twitter.svg')
    );

    this.createAccountForm = this.fb.group({
      firstName: this.fb.control('', Validators.required),
      lastName: this.fb.control('', Validators.required),
      email: this.fb.control('', [Validators.required, Validators.email]),
      password: this.fb.control('', [Validators.required, Validators.minLength(8)]),
      confirmPassword: this.fb.control('', Validators.required),
      acceptTerms: this.fb.control(false, Validators.requiredTrue)
    }, { validators: this.matchPasswords });
  }

  matchPasswords(group: FormGroup) {
    const password = group.get('password')?.value;
    const confirm = group.get('confirmPassword')?.value;
    return password === confirm ? null : { passwordMismatch: true };
  }

  get formControls() {
    return this.createAccountForm.controls;
  }

  signUpWith(provider: string): void {
    console.log(`Sign up with ${provider}`);

  }

  onSubmit(): void {
    let msg = '';
    const form = this.createAccountForm;

    if (form.invalid) {
      form.markAllAsTouched();
      return;
    }

    const rawPassword = form.value.password ?? '';
    const passkey = btoa(rawPassword);

    const payload = {
      FirstName: form.value.firstName,
      LastName: form.value.lastName,
      Emailaddress: form.value.email,
      Passkey: passkey,
      UserName: form.value.email
    };

    this.accountsService.addAccounts(payload).subscribe({
      next: response => {

        msg = 'Account successfully created';

        this.snkbar.open(msg, 'Close', {
          duration: 3000,
          panelClass: ['snackbar-error']
        });

        form.value.password = '';
        form.value.confirmPassword = '';
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
