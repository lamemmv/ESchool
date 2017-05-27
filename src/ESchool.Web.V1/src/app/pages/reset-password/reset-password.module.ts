import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppTranslationModule } from '../../app.translation.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgaModule } from '../../theme/nga.module';

import { ResetPasswordService } from './reset-password.service';
import { ResetPassword } from './reset-password.component';
import { routing } from './reset-password.routing';

@NgModule({
  imports: [
    CommonModule,
    AppTranslationModule,
    ReactiveFormsModule,
    FormsModule,
    NgaModule,
    routing,
  ],
  declarations: [
    ResetPassword,
  ],
  providers: [
    ResetPasswordService,
  ],
})
export class ResetPasswordModule {}
