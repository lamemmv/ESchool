import { NgModule }       from '@angular/core';
import { CommonModule }   from '@angular/common';
import { FormsModule }   from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { AlertModule } from 'ng2-bootstrap';

import { TRANSLATION_PROVIDERS, TranslatePipe, TranslateService }   from './../shared/translate';

import { QuestionTagsComponent } from './question-tags.component';
import { ConfirmDialogComponent } from './../shared/modals/confirm-dialog.component';
import { QuestionTagsService } from './question-tags.service';

@NgModule({
    imports: [
        BrowserModule,
        CommonModule,
        FormsModule,
        AlertModule.forRoot()
    ],
    declarations: [
        QuestionTagsComponent,
        ConfirmDialogComponent,
        TranslatePipe
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