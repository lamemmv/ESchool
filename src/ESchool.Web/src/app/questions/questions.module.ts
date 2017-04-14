import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AlertModule, ModalModule } from 'ng2-bootstrap';
import { CKEditorModule } from 'ng2-ckeditor';
import { SelectModule } from 'ng2-select';
import { TagInputModule } from 'ng2-tag-input';
import { Ng2DropdownModule } from 'ng2-material-dropdown';

import { TRANSLATION_PROVIDERS, TranslateModule, TranslateService }   from './../shared/translate';
import { QuestionsComponent } from './questions.component';
import { EditQuestionComponent } from './question-edit.component';
import { QuestionsService } from './questions.service';
import { UtilitiesService } from './../shared/utils/utilities.service';

import { QuestionsRoutingModule } from './questions-routing.module';

@NgModule({
    imports: [
        CommonModule,
        BrowserModule,
        BrowserAnimationsModule,
        FormsModule,
        ReactiveFormsModule,
        AlertModule.forRoot(),
        ModalModule.forRoot(),
        Ng2DropdownModule,
        TagInputModule,
        TranslateModule,
        CKEditorModule,
        SelectModule,
        QuestionsRoutingModule 
    ],
    declarations: [
        QuestionsComponent,
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