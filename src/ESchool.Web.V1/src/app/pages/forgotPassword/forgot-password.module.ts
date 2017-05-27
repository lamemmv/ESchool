import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppTranslationModule } from '../../app.translation.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgaModule } from '../../theme/nga.module';

import { ForgotPasswordService } from './forgot-password.service';
import { ForgotPassword } from './forgot-password.component';
import { routing } from './forgot-password.routing';

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
    ForgotPassword,
  ],
  providers: [
    ForgotPasswordService,
  ],
})
export class ForgotPasswordModule {}
