import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { environment } from './../../env/environment';  

@Injectable({
  providedIn: 'root'
})

export class AccountsService {

  private baseUrl = environment.apiBaseUrl;
  private loggedIn = new BehaviorSubject<boolean>(false);
  public isLoggedIn$ = this.loggedIn.asObservable();

  constructor(private http: HttpClient) { }

  addAccounts(payload: any): Observable<any> {
    const url = `${this.baseUrl}/api/Accounts/Create`;
    return this.http.post<any>(url,payload);
  }

  resetkey(place: string): Observable<any> {
    const url = `${this.baseUrl}/api/Accounts/Reset/${encodeURIComponent(place)}`;
    return this.http.get<any>(url);
  }

  validateAccounts(place: string): Observable<any> {
    const url = `${this.baseUrl}/api/Accounts/${encodeURIComponent(place)}`;
    return this.http.get<any>(url);
  }

  login(username: string, pass: string): Observable<any> {
    const key = btoa(pass);
    const url = `${this.baseUrl}/api/Accounts/Validate`;
    const payload = {
      username,
      passkey: btoa(pass)  
    };

    return this.http.post<any>(url, payload).pipe(
      tap(() => this.loggedIn.next(true)) 
    );
  }

  logout() {
    this.loggedIn.next(false);
  }

  isLoggedIn(): boolean {
    return this.loggedIn.value;
  }

}

 
