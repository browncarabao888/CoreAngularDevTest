import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AccountsComponent } from './accounts/accounts.component';
import { SearchpageComponent } from './searchpage/searchpage.component';

const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'account', component: AccountsComponent },
  { path: 'searchpage', component: SearchpageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
