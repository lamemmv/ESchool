import { Routes, RouterModule }  from '@angular/router';
import { PagesComponent } from './pages.component';
import { ModuleWithProviders } from '@angular/core';

//import { AuthGuard } from './../security';

export const routes: Routes = [
  {
    path: 'login',
    loadChildren: 'app/pages/login/login.module#LoginModule',
  },
  {
    path: 'forgotPassword',
    loadChildren: 'app/pages/forgot-password/forgot-password.module#ForgotPasswordModule',
  },
  {
    path: 'register',
    loadChildren: 'app/pages/register/register.module#RegisterModule',
  },
  {
    path: 'pages',
    component: PagesComponent,
    children: [
      { path: '', redirectTo: 'home', pathMatch: 'full',
      /*canActivate: [AuthGuard]*/ },
      { path: 'home', loadChildren: './home/home.module#HomeModule',
      /*canLoad: [AuthGuard]*/ },
      { path: 'handbook', loadChildren: './handbook/handbook.module#HandbookModule' },
      { path: 'deviation', loadChildren: './deviation/deviation.module#DeviationModule' },
      { path: 'rm', loadChildren: './rm/rm.module#RiskManagementModule' },
      { path: 'admin', loadChildren: './admin/admin.module#AdminModule' }
    ],
  },
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);
