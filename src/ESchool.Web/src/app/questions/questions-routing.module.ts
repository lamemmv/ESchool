import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { EditQuestionComponent } from './question-edit.component';
import { QuestionListComponent } from './question-list.component';
import { QuestionsComponent } from './questions.component';

const questionRoutes: Routes = [
  {
    path: '', 
    component: QuestionsComponent,
    children: [
      {
        path: '',
        component: QuestionListComponent
      },
      {
        path: 'create',
        component: EditQuestionComponent
      },
      {
        path: 'edit/:id',
        component: EditQuestionComponent
      }
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(questionRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class QuestionsRoutingModule { }
