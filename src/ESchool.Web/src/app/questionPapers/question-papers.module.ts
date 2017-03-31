import { NgModule }       from '@angular/core';
import { FormsModule }   from '@angular/forms';

import { QuestionPapersComponent } from './question-papers.component'
import { QuestionPapersService } from './question-papers.service';

@NgModule({
    imports: [
        FormsModule
    ],
    declarations: [
        QuestionPapersComponent
    ],
    providers: [
        QuestionPapersService
    ]
})
export class QuestionPapersModule { }