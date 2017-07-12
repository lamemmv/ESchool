import { Component } from '@angular/core';

import { LoginService } from './login.service';
import { OidcSecurityCommon } from './../../security/auth';
import { LoginModel } from './login.models';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: [('./login.component.scss')]
})
export class LoginComponent {
  loginModel = new LoginModel();
  constructor(private loginService: LoginService,
    private oidcSecurityCommon: OidcSecurityCommon) {

  }

  onSubmit() {
    this.loginService.login({ username: this.loginModel.userName, password: this.loginModel.password })
      .subscribe((authentication: any) => {
        this.oidcSecurityCommon.store(this.oidcSecurityCommon.storage_access_token, authentication.access_token);
        this.loginService.test().subscribe((res: any) => {
          console.log(res);
        },
        error => {
          console.log(error);
        });
      },
      error => {
        console.log(error);
      });
  }
}
