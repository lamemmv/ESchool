import { NgModule }       from '@angular/core';
import { CommonModule }   from '@angular/common';
import { FormsModule }   from '@angular/forms';

import { QuestionTagsComponent } from './question-tags.component'
import { QuestionTagsService } from './question-tags.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule
    ],
    declarations: [
        QuestionTagsComponent
    ],
    providers: [
        QuestionTagsService
    ]
})
export class QuestionTagsModule { }