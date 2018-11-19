import { Component, OnInit} from '@angular/core';
import { AuthService } from '../auth.service';
import { GS_AUTH_TOKEN, GS_USER_ID } from '../constants';
import { Router } from '@angular/router';
import { Apollo } from 'apollo-angular';
import { SIGNIN_USER_MUTATION, CREATE_USER_MUTATION } from '../graphql';

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

  constructor(private router: Router,
    private authService: AuthService,
    private apollo: Apollo) {
}

  ngOnInit() {
  }

  confirm() {
    if (this.login) {
      this.apollo.mutate({
        mutation: SIGNIN_USER_MUTATION,
        variables: {
          user: {
            email: this.email,
            password: this.password
          }
        }
      }).subscribe((result) => {
        const id = result.data.signinUser.user.id;
        const token = result.data.signinUser.token;
        this.saveUserData(id, token);

        this.router.navigate(['/']);

      }, (error) => {
        alert(error);
      });
    } else {
      this.apollo.mutate({
        mutation: CREATE_USER_MUTATION,
        variables: {
          user: {
            name: this.name,
            email: this.email,
            password: this.password
          }
        }
      }).subscribe((result) => {
        const id = result.data.signinUser.user.id;
        const token = result.data.signinUser.token;
        this.saveUserData(id, token);

        this.router.navigate(['/']);

      }, (error) => {
        alert(error);
      });
    }
  }

  saveUserData(id, token) {
    localStorage.setItem(GS_USER_ID, id);
    localStorage.setItem(GS_AUTH_TOKEN, token);
    this.authService.setUserId(id);
  }
}
