import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { RouterModule,  PreloadAllModules} from '@angular/router';

// Third parties
import { Ng2BootstrapModule } from 'ng2-bootstrap';
import { Ng2BreadcrumbModule, BreadcrumbService } from 'ng2-breadcrumb/ng2-breadcrumb';
import { BootstrapModalModule } from 'ng2-bootstrap-modal';

import { TRANSLATION_PROVIDERS, TranslatePipe, TranslateService, TranslateModule }   from './shared/translate';
import { ConfigService } from './shared/utils/config.service';
import { NotificationService } from './shared/utils/notification.service';
import { AppService } from './shared/app.service';
//import { QuestionTagsModule } from './questionTags/question-tags.module';
//import { QuestionsModule } from './questions/questions.module';
import { AdminModule } from './admin/admin.module';
import { QuestionPapersModule } from './questionPapers/question-papers.module';
import { PageNotFoundComponent } from './errors/page-not-found.component';
import { AppComponent }  from './app.component';
import { HomeComponent }  from './home/home.component';
import { AppRoutingModule } from './app-routing.module';
import { LoginRoutingModule } from './login/login-routing.module';
import { LoginComponent } from './login/login.component';

@NgModule({
  imports: [ 
    BrowserModule,
    HttpModule,
    FormsModule,
    Ng2BootstrapModule.forRoot(),
    Ng2BreadcrumbModule.forRoot(),
    BootstrapModalModule,
    AdminModule,
    //QuestionTagsModule,
    //QuestionsModule,
    QuestionPapersModule,
    TranslateModule,
    LoginRoutingModule,
    AppRoutingModule
  ], declarations: [ 
    AppComponent,
    HomeComponent,
    LoginComponent,
    PageNotFoundComponent
  ], providers: [
    ConfigService,
    NotificationService,
    AppService,
    BreadcrumbService,
    TRANSLATION_PROVIDERS, 
    TranslateService
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
