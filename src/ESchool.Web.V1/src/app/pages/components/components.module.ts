import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgaModule } from '../../theme/nga.module';
import { TreeModule } from 'ng2-tree';
import { AlertModule, ModalModule, DatepickerModule } from 'ng2-bootstrap';
import { TreeTableModule } from 'primeng/primeng';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { CKEditorModule } from 'ng2-ckeditor';
import { RatingModule } from 'ngx-rating';

import { AppTranslationModule } from '../../app.translation.module';
import { routing } from './components.routing';
import { Components } from './components.component';
import { TreeView } from './components/treeView/treeView.component';
import { GroupsService } from './components/groups/groups.service';
import { QuestionTagsService } from './components/questionTags/question-tags.service';
import { QuestionTagsComponent } from './components/questionTags/question-tags.component';
import { EditQuestionTagComponent } from './components/questionTags/question-tag-edit.component';
import { QuestionsService } from './components/questions/questions.service';
import { QuestionsComponent } from './components/questions/questions.component';
import { EditQuestionComponent } from './components/questions/edit/question-edit.component';
import { QuestionListComponent } from './components/questions/list/question-list.component';
import { ConfirmDeleteQuestionComponent } from './components/questions/delete/confirm-delete.component';
import { ClickOutsideModule, DatepickerComponent } from './../../shared/datePicker';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    NgaModule,
    TreeModule,
    AlertModule.forRoot(),
    ModalModule.forRoot(),
    DatepickerModule.forRoot(),
    ClickOutsideModule,
    TreeTableModule,
    NgbModalModule,
    CKEditorModule,
    RatingModule,
    AppTranslationModule,
    routing
  ],
  declarations: [
    DatepickerComponent,
    Components,
    TreeView,
    QuestionTagsComponent,
    EditQuestionTagComponent,
    QuestionsComponent,
    EditQuestionComponent,
    QuestionListComponent,
    ConfirmDeleteQuestionComponent
  ],
  entryComponents: [
    EditQuestionTagComponent,
    ConfirmDeleteQuestionComponent
  ],
  providers: [
      QuestionTagsService,
      GroupsService,
      QuestionsService
  ]
})
export class ComponentsModule { }
