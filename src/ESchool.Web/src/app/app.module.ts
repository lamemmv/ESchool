import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import {
  RouterModule,
  PreloadAllModules
} from '@angular/router';
import { Ng2BootstrapModule } from 'ng2-bootstrap';
import { Ng2BreadcrumbModule, BreadcrumbService } from 'ng2-breadcrumb/ng2-breadcrumb';
import { BootstrapModalModule } from 'ng2-bootstrap-modal';

import { TRANSLATION_PROVIDERS, TranslatePipe, TranslateService, TranslateModule }   from './shared/translate';

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
        Ng2BreadcrumbModule.forRoot(),
        BootstrapModalModule,
        QuestionTagsModule,
        QuestionsModule,
        QuestionPapersModule,
        TranslateModule
    ],
  declarations: [ AppComponent,
        HomeComponent
    ],
    providers: [
        ConfigService,
        NotificationService,
        AppService,
        BreadcrumbService,
        TRANSLATION_PROVIDERS, 
        TranslateService
    ],
  bootstrap:    [ AppComponent ]
})
export class AppModule { }
