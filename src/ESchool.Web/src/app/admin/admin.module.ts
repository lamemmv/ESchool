import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { TRANSLATION_PROVIDERS, TranslatePipe, TranslateService, TranslateModule }   from './../shared/translate';

import { AdminComponent } from './admin.component';
import { AdminHomeComponent } from './home/admin-home.component';
import { QuestionTagsModule } from './../questionTags/question-tags.module';
import { QuestionsModule } from './../questions/questions.module';
import { AdminRoutingModule } from './admin-routing.module';

@NgModule({
    imports: [
        CommonModule,
        BrowserModule,
        BrowserAnimationsModule,
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