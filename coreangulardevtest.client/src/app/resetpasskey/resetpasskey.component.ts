import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar, MatSnackBarRef, TextOnlySnackBar } from '@angular/material/snack-bar';
import { AccountsService } from '../accounts/accounts.service';


@Component({
  selector: 'app-resetpasskey',
  templateUrl: './resetpasskey.component.html',
  styleUrl: './resetpasskey.component.css'
})
export class ResetpasskeyComponent {
  ResetForm: FormGroup;

  constructor(
    private accountsService: AccountsService,
    private fb: FormBuilder,
    private snkbar: MatSnackBar
  ) {
    this.ResetForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
      
    });
  }



  get email() {
    return this.ResetForm.get('email');
  }

  onReset(): void {

    const form = this.ResetForm;
    const emailadd = form.value.email;
    let msg = '';

    if (form.invalid) {
      form.markAllAsTouched();
      return;
    }

    this.accountsService.resetkey(emailadd).subscribe({
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
