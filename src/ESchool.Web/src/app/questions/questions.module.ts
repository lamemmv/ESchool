import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';

import { AlertModule, ModalModule as BootstrapModalModule } from 'ng2-bootstrap';
import { CKEditorModule } from 'ng2-ckeditor';
import { TagInputModule } from 'ng2-tag-input';
import { RatingModule } from 'ngx-rating';
import { ModalModule } from "ngx-modal";

import { TRANSLATION_PROVIDERS, TranslateModule, TranslateService }   from './../shared/translate';
import { QuestionsComponent } from './questions.component';
import { EditQuestionComponent } from './question-edit.component';
import { QuestionListComponent } from './question-list.component';
import { QuestionsService } from './questions.service';
import { UtilitiesService } from './../shared/utils/utilities.service';

import { QuestionsRoutingModule } from './questions-routing.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        AlertModule.forRoot(),
        BootstrapModalModule.forRoot(),
        ModalModule,
        TagInputModule,
        RatingModule,
        TranslateModule,
        CKEditorModule,
        QuestionsRoutingModule 
    ],
    declarations: [
        QuestionsComponent,
        QuestionListComponent,
        EditQuestionComponent
    ],
    providers: [
        QuestionsService,
        TRANSLATION_PROVIDERS, 
        TranslateService,
        UtilitiesService
    ]
})
export class QuestionsModule { }