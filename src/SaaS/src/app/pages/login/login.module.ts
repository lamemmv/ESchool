import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';

import { routing } from './login.routing.module';
import { LoginComponent } from './login.component';
import { LoginService } from './login.service';
import { AppTranslationModule } from './../../translation.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        AppTranslationModule,
        routing
    ],
    declarations: [
        LoginComponent
    ],
    providers: [
        LoginService
    ]
})
export class LoginModule {
}
