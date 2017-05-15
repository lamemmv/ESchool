import { Routes, RouterModule }  from '@angular/router';

import { Components } from './components.component';
import { TreeView } from './components/treeView/treeView.component';
import { QuestionTagsComponent } from './components/questionTags/question-tags.component';
import { QuestionsComponent } from './components/questions/questions.component';
import { EditQuestionComponent } from './components/questions/edit/question-edit.component';
import { QuestionListComponent } from './components/questions/list/question-list.component';

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
          }
        ] 
      }
    ]
  }
];

export const routing = RouterModule.forChild(routes);
