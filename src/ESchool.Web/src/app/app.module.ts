import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import {
  RouterModule,
  PreloadAllModules
} from '@angular/router';
import { Ng2BootstrapModule } from 'ng2-bootstrap';
import { ModalModule } from 'angular2-modal';

//import { TRANSLATION_PROVIDERS, TranslatePipe, TranslateService }   from './shared/translate';

import { ConfigService } from './shared/utils/config.service';
import { NotificationService } from './shared/utils/notification.service';
import { AppService } from './shared/app.service';
import { QuestionTagsModule } from './questionTags/question-tags.module';
import { QuestionsModule } from './questions/questions.module';
import { QuestionPapersModule } from './questionPapers/question-papers.module';
import { AppComponent }  from './app.component';
import { HomeComponent }  from './home/home.component';
import { ROUTES } from './app.routes';

@NgModule({
  imports:      [ BrowserModule,
        HttpModule,
        RouterModule.forRoot(ROUTES, { useHash: false, preloadingStrategy: PreloadAllModules }),
        Ng2BootstrapModule.forRoot(),
        ModalModule.forRoot(),
        QuestionTagsModule,
        QuestionsModule,
        QuestionPapersModule 
    ],
  declarations: [ AppComponent,
        HomeComponent,
        //TranslatePipe 
    ],
    providers: [
        ConfigService,
        NotificationService,
        AppService,
        //TRANSLATION_PROVIDERS, 
        //TranslateService
    ],
  bootstrap:    [ AppComponent ]
})
export class AppModule { }
