import { NgModule }       from '@angular/core';
import { FormsModule }   from '@angular/forms';

import { QuestionsComponent } from './questions.component'
import { QuestionsService } from './questions.service';

@NgModule({
    imports: [
        FormsModule
    ],
    declarations: [
        QuestionsComponent
    ],
    providers: [
        QuestionsService
    ]
})
export class QuestionsModule { }