import { Routes, RouterModule } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { ForgotPassword } from './forgot-password.component';

// noinspection TypeScriptValidateTypes
export const routes: Routes = [
  {
    path: '',
    component: ForgotPassword,
  },
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);
