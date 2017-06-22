import { NgModule } from '@angular/core';

import { routing } from './pages.routing.module';
import { PagesComponent } from './pages.component';

@NgModule({
    imports: [
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
