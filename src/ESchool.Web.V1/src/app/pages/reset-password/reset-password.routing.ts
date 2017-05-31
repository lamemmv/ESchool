import { Routes, RouterModule } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { ResetPasswordComponent } from './reset-password.component';

// noinspection TypeScriptValidateTypes
export const routes: Routes = [
  {
    path: '',
    component: ResetPasswordComponent,
  }
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);
