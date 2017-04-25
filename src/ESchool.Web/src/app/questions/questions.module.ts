import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AlertModule, ModalModule as BootstrapModalModule } from 'ng2-bootstrap';
import { CKEditorModule } from 'ng2-ckeditor';
import { TagInputModule } from 'ng2-tag-input';
import { Ng2DropdownModule } from 'ng2-material-dropdown';
import { RatingModule } from 'ngx-rating';
import { ModalModule } from "ngx-modal";
import { FileUploadModule } from 'ng2-file-upload';

import { TRANSLATION_PROVIDERS, TranslateModule, TranslateService }   from './../shared/translate';
import { QuestionsComponent } from './questions.component';
import { EditQuestionComponent } from './question-edit.component';
import { UploadFileComponent } from './../shared/upload/upload-file.component';
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
        BootstrapModalModule.forRoot(),
        ModalModule,
        Ng2DropdownModule,
        TagInputModule,
        RatingModule,
        FileUploadModule,
        TranslateModule,
        CKEditorModule,
        QuestionsRoutingModule 
    ],
    declarations: [
        QuestionsComponent,
        EditQuestionComponent,
        UploadFileComponent
    ],
    providers: [
        QuestionsService,
        TRANSLATION_PROVIDERS, 
        TranslateService,
        UtilitiesService
    ]
})
export class QuestionsModule { }