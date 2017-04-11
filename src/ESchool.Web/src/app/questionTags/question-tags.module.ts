import { NgModule }       from '@angular/core';
import { CommonModule }   from '@angular/common';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';

import { AlertModule, ModalModule } from 'ng2-bootstrap';

import { TRANSLATION_PROVIDERS, TranslateModule, TranslateService }   from './../shared/translate';

import { QuestionTagsComponent } from './question-tags.component';
import { ConfirmDialogComponent } from './../shared/modals/confirm-dialog.component';
import { QuestionTagsService } from './question-tags.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        AlertModule.forRoot(),
        ModalModule.forRoot(),
        TranslateModule
    ],
    declarations: [
        QuestionTagsComponent,
        ConfirmDialogComponent        
    ],
    providers: [
        QuestionTagsService,
        TRANSLATION_PROVIDERS, 
        TranslateService
    ],
    entryComponents: [
        ConfirmDialogComponent
  ]
})
export class QuestionTagsModule { }