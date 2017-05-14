import { Routes, RouterModule }  from '@angular/router';

import { Components } from './components.component';
import { TreeView } from './components/treeView/treeView.component';
import { QuestionTagsComponent } from './components/questionTags/question-tags.component';

// noinspection TypeScriptValidateTypes
const routes: Routes = [
  {
    path: '',
    component: Components,
    children: [
      { path: 'treeview', component: TreeView },
      { path: 'questionTags', component: QuestionTagsComponent }
    ]
  }
];

export const routing = RouterModule.forChild(routes);
