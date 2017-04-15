import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { QuestionPapersComponent } from './question-papers.component';
import { AuthGuard }                from './../shared/authentications/auth-guard.service';

const questionPapersRoutes: Routes = [
  {
    path: 'questionPapers',
    component: QuestionPapersComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(questionPapersRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class QuestionPapersRoutingModule {}
