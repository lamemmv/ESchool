import { Routes, RouterModule } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { ResetPassword } from './reset-password.component';

// noinspection TypeScriptValidateTypes
export const routes: Routes = [
  {
    path: '',
    component: ResetPassword,
  }
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);
