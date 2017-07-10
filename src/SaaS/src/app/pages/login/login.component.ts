import { Component } from '@angular/core';

import { LoginService } from './login.service';
import { OidcSecurityCommon } from './../../security/auth';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: [('./login.component.scss')]
})
export class LoginComponent {
  constructor(private loginService: LoginService,
    private oidcSecurityCommon: OidcSecurityCommon) {

  }

  submit() {
    this.loginService.login({ username: 'lam', password: 'password' })
      .subscribe((authentication: any) => {
        this.oidcSecurityCommon.store(this.oidcSecurityCommon.storage_access_token, authentication.access_token);
      },
      error => {
        console.log(error);
      });
  }
}
