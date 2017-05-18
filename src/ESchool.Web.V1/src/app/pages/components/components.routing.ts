import { Routes, RouterModule }  from '@angular/router';

import { Components } from './components.component';
import { TreeView } from './components/treeView/treeView.component';
import { QuestionTagsComponent } from './components/questionTags';
import { QuestionsComponent, EditQuestionComponent,
  QuestionListComponent } from './components/questions';
import { ExamPapersComponent, EditExamPaperComponent,
  ExamPaperListComponent } from './components/examPapers';
// noinspection TypeScriptValidateTypes
const routes: Routes = [
  {
    path: '',
    component: Components,
    children: [
      { path: 'treeview', component: TreeView },
      { path: 'questionTags', component: QuestionTagsComponent },
      { path: 'questions', component: QuestionsComponent,
        children: [
          {
            path: '',
            component: QuestionListComponent
          },
          {
            path: 'edit/:id',
            component: EditQuestionComponent
          },
          {
            path: 'create',
            component: EditQuestionComponent
          },
        ], 
      },
      { path: 'examPapers', component: ExamPapersComponent,
        children: [
          {
            path: '',
            component: ExamPaperListComponent
          },
          {
            path: 'edit/:id',
            component: ExamPaperListComponent
          },
          {
            path: 'create',
            component: ExamPaperListComponent
          },
        ], 
      },
    ],
  }
];

export const routing = RouterModule.forChild(routes);
