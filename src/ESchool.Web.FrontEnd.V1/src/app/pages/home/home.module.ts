import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MnFullpageModule } from 'ngx-fullpage';

import { routing } from './home.routing';
import { HomeComponent } from './home.component';

@NgModule({
    imports: [CommonModule,
        MnFullpageModule.forRoot(),
        routing],
    declarations: [HomeComponent],
    providers: []
})
export class HomeModule {
}
