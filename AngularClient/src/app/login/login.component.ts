import {Component, OnInit} from '@angular/core';
import { AuthService } from '../auth.service';
import { GS_AUTH_TOKEN, GS_USER_ID } from '../constants';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  login = true; // switch between Login and SignUp
  email = '';
  password = '';
  name = '';

  constructor(private authService: AuthService) {
  }

  ngOnInit() {
  }

  confirm() {
    // ... you'll implement this in a bit
  }

  saveUserData(id, token) {
    localStorage.setItem(GS_USER_ID, id);
    localStorage.setItem(GS_AUTH_TOKEN, token);
    this.authService.setUserId(id);
  }
}
