import { NgModule }       from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';

import { AlertModule, ModalModule } from 'ng2-bootstrap';
import { CKEditorModule } from 'ng2-ckeditor';

import { TRANSLATION_PROVIDERS, TranslateModule, TranslateService }   from './../shared/translate';
import { QuestionsComponent } from './questions.component'
import { QuestionsService } from './questions.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        AlertModule.forRoot(),
        ModalModule.forRoot(),
        TranslateModule,
        CKEditorModule 
    ],
    declarations: [
        QuestionsComponent
    ],
    providers: [
        QuestionsService,
        TRANSLATION_PROVIDERS, 
        TranslateService
    ]
})
export class QuestionsModule { }