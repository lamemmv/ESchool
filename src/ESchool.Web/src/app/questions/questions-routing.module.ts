import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

//import { CreateQuestionComponent } from './question-create.component';
import { EditQuestionComponent } from './question-edit.component';
import { QuestionsComponent } from './questions.component';

const questionRoutes: Routes = [
  { path: 'questions', component: QuestionsComponent },
  { path: 'question/create', component: EditQuestionComponent },
  { path: 'question/edit/:id', component: EditQuestionComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(questionRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class QuestionsRoutingModule {}
