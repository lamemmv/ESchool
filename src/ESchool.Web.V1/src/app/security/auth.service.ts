import { Injectable } from '@angular/core';
import { Headers } from '@angular/http';

import 'rxjs/add/observable/of';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/delay';

import { Authentication } from './auth.model';

@Injectable()
export class AuthService {
  isLoggedIn: boolean = false;

  // store the URL so we can redirect after logging in
  redirectUrl: string;

  // simple check of logged in status: if there is a token, we're (probably) logged in.
  // ideally we check status and check token has not expired (server will back us up, if this not done, but it could be cleaner)
  loggedIn(): boolean {
    return !!sessionStorage.getItem('bearer_token');
  }

  // for requesting secure data using json
  authJsonHeaders() {
    const header = new Headers();
    header.append('Content-Type', 'application/json');
    header.append('Accept', 'application/json');
    header.append('Authorization', 'Bearer ' + sessionStorage.getItem('bearer_token'));
    return header;
  }

  // for requesting secure data from a form post
  authFormHeaders() {
    const header = new Headers();
    header.append('Content-Type', 'application/x-www-form-urlencoded');
    header.append('Accept', 'application/json');
    header.append('Authorization', 'Bearer ' + sessionStorage.getItem('bearer_token'));
    return header;
  }

  // for requesting unsecured data using json
  jsonHeaders() {
    const header = new Headers();
    header.append('Content-Type', 'application/json');
    header.append('Accept', 'application/json');
    return header;
  }

  // for requesting unsecured data using form post
  contentHeaders() {
    const header = new Headers();
    header.append('Content-Type', 'application/x-www-form-urlencoded');
    header.append('Accept', 'application/json');
    return header;
  }

  // After a successful login, save token data into session storage
  // note: use "localStorage" for persistent, browser-wide logins; "sessionStorage" for per-session storage.
  login(auth: Authentication) {
    sessionStorage.setItem('access_token', auth.accessToken);
    sessionStorage.setItem('bearer_token', auth.accessToken);

    // TODO: implement meaningful refresh, handle expiry 
    sessionStorage.setItem('expires_in', auth.expiresIn.toString());
  }

  // called when logging out user; clears tokens from browser
  logout() {
    sessionStorage.removeItem('access_token');
    sessionStorage.removeItem('bearer_token');
    sessionStorage.removeItem('expires_in');
  }

}
