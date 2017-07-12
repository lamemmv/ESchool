import { NgModule } from '@angular/core';

import { routing } from './pages.routing.module';
import { PagesComponent } from './pages.component';
import { AppTranslationModule } from './../translation.module';

@NgModule({
    imports: [
        AppTranslationModule,
        routing
    ],
    declarations: [
        PagesComponent
    ],
    providers: [
    ]
})
export class PagesModule {
}
