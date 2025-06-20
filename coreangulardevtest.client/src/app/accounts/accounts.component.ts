import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';

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
    iconRegistry: MatIconRegistry,
    sanitizer: DomSanitizer,
    private fb: FormBuilder
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
      password: this.fb.control('', [Validators.required, Validators.minLength(6)]),
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
    // Integrate with OAuth/Firebase/etc.
  }

  onSubmit(): void {
  }
}
