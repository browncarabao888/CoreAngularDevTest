import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from '@angular/cdk/layout';

import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatAutocompleteModule } from '@angular/material/autocomplete';

import { FormsModule } from '@angular/forms';
import { MatRadioModule } from '@angular/material/radio'; 
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule } from '@angular/material/dialog';
import { DashboardComponent } from './dashboard/dashboard.component'; 
import { AppRoutingModule } from './app-routing.module';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { AccountsComponent } from './accounts/accounts.component';
import { SearchpageComponent } from './searchpage/searchpage.component';

import { GoogleMapsModule } from '@angular/google-maps';
import { PlacedetailsComponent } from './placedetails/placedetails.component';
import { LoginComponent } from './login/login.component';
import { ResetpasskeyComponent } from './resetpasskey/resetpasskey.component';
 


@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    AccountsComponent,
    SearchpageComponent,
    PlacedetailsComponent,
    LoginComponent,
    ResetpasskeyComponent 
  
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    LayoutModule,
    MatSnackBarModule,
    MatAutocompleteModule,
    MatToolbarModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule,
    MatRadioModule,
    MatSelectModule,
    MatDialogModule,
    FormsModule,
    GoogleMapsModule,
    MatProgressSpinnerModule,
    MatChipsModule,
      HttpClientModule,
    AppRoutingModule
  ],
  bootstrap: [AppComponent]
})


export class AppModule {
  constructor(iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) {
  iconRegistry.addSvgIcon('google-icon', sanitizer.bypassSecurityTrustResourceUrl('assets/icons/google.svg'));
  iconRegistry.addSvgIcon('facebook-icon', sanitizer.bypassSecurityTrustResourceUrl('assets/icons/facebook.svg'));
  iconRegistry.addSvgIcon('twitter-icon', sanitizer.bypassSecurityTrustResourceUrl('assets/icons/twitter.svg'));
}
}
