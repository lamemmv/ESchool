import { Component } from '@angular/core';
import { FormGroup, AbstractControl, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, Params} from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

import { ResetPasswordModel } from './reset-password.model';
import { ResetPasswordService } from './reset-password.service';
import { NotificationService } from './../../shared/utils/notification.service';
import { AuthService, Authentication } from './../../security';

@Component({
  selector: 'reset-password',
  templateUrl: './reset-password.html',
  styleUrls: ['./reset-password.scss'],
})
export class ResetPasswordComponent {

  public form: FormGroup;
  public email: AbstractControl;
  public password: AbstractControl;
  public confirmPassword: AbstractControl;
  public recoverPasswordSuccessful: boolean = false;
  private model = new ResetPasswordModel();
  private recoverPasswordSuccessfulMessage: string;
  constructor(fb: FormBuilder,
    private resetPasswordService: ResetPasswordService,
    private notificationService: NotificationService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private _translate: TranslateService) {
    this.form = fb.group({
      'email': ['', Validators.compose([Validators.required, Validators.minLength(4)])],
      'password': ['', Validators.compose([Validators.required, Validators.minLength(4)])],
      'confirmPassword': ['', Validators.compose([Validators.required, Validators.minLength(4)])]
    });

    this.email = this.form.controls['email'];
    this.password = this.form.controls['password'];
    this.confirmPassword = this.form.controls['confirmPassword'];
    this.route.queryParams.subscribe((params: Params) => {
        this.model.userId = params['userId'];
        this.model.code = params['code'];
      });

    this._translate.get(['RECOVER_PASSWORD_SUCCESSFUL', 'login.sign_in']).subscribe((res: any) => {
        this.recoverPasswordSuccessfulMessage = String.format(res.RECOVER_PASSWORD_SUCCESSFUL, 
          String.format('<a href="/#/login">{0}</a>', res['login.sign_in']));
    });
  }

  public onSubmit(values: Object): void {
    if (this.form.valid) {      
      this.model.email = this.email.value;
      this.model.password = this.password.value;
      this.model.confirmPassword = this.confirmPassword.value;
      this.resetPasswordService.forgotPassword(this.model)
      .subscribe((response: any) => {
        this.recoverPasswordSuccessful = true;
      },
      error => {
        this.notificationService.printErrorMessage('Failed to change your password');
      });
    }
  }
}
