import { NgModule }       from '@angular/core';
import { FormsModule }   from '@angular/forms';

import { QuestionPapersComponent } from './question-papers.component'
import { QuestionPapersService } from './question-papers.service';
import { AuthService } from './../shared/authentications/auth.service';
import { AuthGuard } from './../shared/authentications/auth-guard.service';
import { QuestionPapersRoutingModule }       from './question-papers-routing.module';

@NgModule({
    imports: [
        FormsModule,
        QuestionPapersRoutingModule
    ],
    declarations: [
        QuestionPapersComponent
    ],
    providers: [
        AuthService,
        AuthGuard,
        QuestionPapersService
    ]
})
export class QuestionPapersModule { }