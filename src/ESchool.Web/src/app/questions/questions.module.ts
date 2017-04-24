import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AlertModule } from 'ng2-bootstrap';
import { CKEditorModule } from 'ng2-ckeditor';
import { TagInputModule } from 'ng2-tag-input';
import { Ng2DropdownModule } from 'ng2-material-dropdown';
import { RatingModule } from 'ngx-rating';
import { NgUploaderModule } from 'ngx-uploader';
import { ModalModule } from "ngx-modal";

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
        ModalModule,
        Ng2DropdownModule,
        TagInputModule,
        RatingModule,
        NgUploaderModule,
        TranslateModule,
        CKEditorModule,
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