import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';

import { TRANSLATION_PROVIDERS, TranslatePipe, TranslateService, TranslateModule }   from './../shared/translate';

import { AdminComponent } from './admin.component';
import { AdminHomeComponent } from './home/admin-home.component';
import { QuestionTagsModule } from './../questionTags/question-tags.module';
import { QuestionsModule } from './../questions/questions.module';
import { AdminRoutingModule } from './admin-routing.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        TranslateModule,
        QuestionTagsModule,
        QuestionsModule,
        AdminRoutingModule
    ],
    declarations: [
        AdminComponent,
        AdminHomeComponent
    ],
    providers: [
        TRANSLATION_PROVIDERS,
        TranslateService
    ]
})
export class AdminModule { }