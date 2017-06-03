import { Component } from '@angular/core';
import { FormGroup, AbstractControl, FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';

import { ForgotPasswordModel } from './forgot-password.model';
import { ForgotPasswordService } from './forgot-password.service';
import { NotificationService } from './../../shared/utils/notification.service';
import { AuthService, Authentication } from './../../security';

@Component({
  selector: 'forgot-password',
  templateUrl: './forgot-password.html',
  styleUrls: ['./forgot-password.scss']
})
export class ForgotPassword {

  public form: FormGroup;
  public email: AbstractControl;
  public password: AbstractControl;
  public resetPasswordSucceed: boolean = false;
  private backToLoginPageMessage: string;
  constructor(fb: FormBuilder,
    private forgotPasswordService: ForgotPasswordService,
    private notificationService: NotificationService,
    private authService: AuthService,
    private _translate: TranslateService) {
    this.form = fb.group({
      'email': ['', Validators.compose([Validators.required, Validators.minLength(4)])]
    });

    this.email = this.form.controls['email'];
    this._translate.get(['BACK_TO_LOGIN_PAGE', 'login.sign_in']).subscribe((res: any) => {
        this.backToLoginPageMessage = String.format(res.BACK_TO_LOGIN_PAGE, 
          String.format('<a href="/#/login">{0}</a>', res['login.sign_in']));
    });
  }

  public onSubmit(values: Object): void {    
    if (this.form.valid) {
      const model = new ForgotPasswordModel();
      model.email = this.email.value;
      model.url = 'http://localhost:4200/#/resetPassword';
      this.forgotPasswordService.forgotPassword(model)
      .subscribe((response: any) => {
        this.resetPasswordSucceed = true;
      },
      error => {
        this.notificationService.printErrorMessage('Failed to recover your password');
      });
    }
  }
}
