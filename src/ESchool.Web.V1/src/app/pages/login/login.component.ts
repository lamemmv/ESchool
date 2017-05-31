import { Component } from '@angular/core';
import { FormGroup, AbstractControl, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { LoginModel } from './login.model';
import { LoginService } from './login.service';
import { NotificationService } from './../../shared/utils/notification.service';
import { AuthService, Authentication } from './../../security';

@Component({
  selector: 'login',
  templateUrl: './login.html',
  styleUrls: ['./login.scss']
})
export class Login {

  public form: FormGroup;
  public email: AbstractControl;
  public password: AbstractControl;
  public submitted: boolean = false;

  constructor(fb: FormBuilder,
    private loginService: LoginService,
    private notificationService: NotificationService,
    private authService: AuthService,
    private router: Router) {
    this.form = fb.group({
      'email': ['', Validators.compose([Validators.required, Validators.minLength(4)])],
      'password': ['', Validators.compose([Validators.required, Validators.minLength(4)])]
    });

    this.email = this.form.controls['email'];
    this.password = this.form.controls['password'];
  }

  public onSubmit(values: Object): void {
    this.submitted = true;
    if (this.form.valid) {
      const model = new LoginModel();
      model.username = this.email.value;
      model.password = this.password.value;
      this.loginService.login(model)
      .subscribe((token: any) => {
        const authentication = new Authentication();
        authentication.accessToken = token.access_token;
        authentication.expiresIn = token.expires_in;
        authentication.resource = token.resource;
        authentication.tokenType = token.token_type;
        this.authService.login(authentication);
        const redirect = this.authService.redirectUrl ? this.authService.redirectUrl : '/pages/dashboard';
        // Redirect the user
        this.router.navigate([redirect]);
      },
      error => {
        this.notificationService.printErrorMessage('Failed to login');
      });
    }
  }
}
