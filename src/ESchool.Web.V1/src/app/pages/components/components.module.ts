import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgaModule } from '../../theme/nga.module';
import { TreeModule } from 'ng2-tree';
import { AlertModule, ModalModule, DatepickerModule } from 'ng2-bootstrap';
import { TreeTableModule } from 'primeng/primeng';
import { NgbModalModule, NgbRatingModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { CKEditorModule } from 'ng2-ckeditor';

import { ClickOutsideModule, DatepickerComponent } from './../../shared/datePicker';
import { AppTranslationModule } from '../../app.translation.module';
import { routing } from './components.routing';
import { Components } from './components.component';
import { TreeView } from './components/treeView/treeView.component';
import { GroupsService } from './components/groups/groups.service';
import {
  QuestionTagsService,
  EditQuestionTagComponent, QuestionTagsComponent
} from './components/questionTags';
import {
  QuestionsService, QuestionsComponent,
  QUploadFileComponent, EditQuestionComponent,
  ConfirmDeleteQuestionComponent, QuestionListComponent,
} from './components/questions';
import {
  ExamPapersService, ExamPapersComponent,
  EditExamPaperComponent, ConfirmDeleteExamPaperComponent,
  ExamPaperListComponent, ExamPaperPartComponent
} from './components/examPapers';


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
    NgbRatingModule,
    NgbPaginationModule,
    AppTranslationModule,
    routing,
  ],
  declarations: [
    DatepickerComponent,
    Components,
    TreeView,
    EditQuestionTagComponent,
    QuestionTagsComponent,
    QuestionsComponent,
    EditQuestionComponent,
    QuestionListComponent,
    ConfirmDeleteQuestionComponent,
    QUploadFileComponent,
    ExamPapersComponent,
    ConfirmDeleteExamPaperComponent,
    ExamPaperPartComponent,
    EditExamPaperComponent,
    ExamPaperListComponent,
  ],
  entryComponents: [
    EditQuestionTagComponent,
    ConfirmDeleteQuestionComponent,
    QUploadFileComponent,
    ExamPaperPartComponent,
    ConfirmDeleteExamPaperComponent,
  ],
  providers: [
    GroupsService,
    QuestionTagsService,
    QuestionsService,
    ExamPapersService,
  ]
})
export class ComponentsModule { }
