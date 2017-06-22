import { Routes, RouterModule } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';
//import { AuthGuard } from './security';

export const routes: Routes = [
  { path: '', redirectTo: 'pages', pathMatch: 'full' },
  { path: '**', redirectTo: 'pages/home'/*, canActivate: [AuthGuard]*/ }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(routes, { useHash: true });
