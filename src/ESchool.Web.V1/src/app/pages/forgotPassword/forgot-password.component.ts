import { Component } from '@angular/core';
import { FormGroup, AbstractControl, FormBuilder, Validators } from '@angular/forms';

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
  public submitted: boolean = false;

  constructor(fb: FormBuilder,
    private forgotPasswordService: ForgotPasswordService,
    private notificationService: NotificationService,
    private authService: AuthService) {
    this.form = fb.group({
      'email': ['', Validators.compose([Validators.required, Validators.minLength(4)])]
    });

    this.email = this.form.controls['email'];
  }

  public onSubmit(values: Object): void {
    this.submitted = true;
    if (this.form.valid) {
      const model = new ForgotPasswordModel();
      model.email = this.email.value;
      model.url = 'http://localhost:4200/resetPassword';
      this.forgotPasswordService.forgotPassword(model)
      .subscribe((response: any) => {
        console.log(response);
      },
      error => {
        this.notificationService.printErrorMessage('Failed to recover your password');
      });
    }
  }
}
