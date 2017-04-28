import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';

import { TRANSLATION_PROVIDERS, TranslatePipe, TranslateService, TranslateModule }   from './../shared/translate';

import { ExamPapersComponent } from './exam-papers.component';
import { ExamPapersRoutingModule } from './exam-papers-routing.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        TranslateModule,
        ExamPapersRoutingModule
    ],
    declarations: [
        ExamPapersComponent
    ],
    providers: [
        TRANSLATION_PROVIDERS,
        TranslateService
    ]
})
export class ExamPapersModule { }