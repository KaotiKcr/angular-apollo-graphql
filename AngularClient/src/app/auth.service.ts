import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';
import { GS_AUTH_TOKEN, GS_USER_ID } from './constants';

@Injectable()
export class AuthService {
  private userId: string = null;
  private _isAuthenticated = new BehaviorSubject(false);

  constructor() {}

  get isAuthenticated(): Observable<any> {
    return this._isAuthenticated.asObservable();
  }
  saveUserData(id: string, token: string) {
    localStorage.setItem(GS_USER_ID, id);
    localStorage.setItem(GS_AUTH_TOKEN, token);
    this.setUserId(id);
  }
  setUserId(id: string) {
    this.userId = id;
    this._isAuthenticated.next(true);
  }
  logout() {
    localStorage.removeItem(GS_USER_ID);
    localStorage.removeItem(GS_AUTH_TOKEN);
    this.userId = null;
    this._isAuthenticated.next(false);
  }
  autoLogin() {
    const id = localStorage.getItem(GS_USER_ID);
    if (id) {
      this.setUserId(id);
    }
  }
}
