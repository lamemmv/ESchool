import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { routing } from './home.routing';
import { HomeComponent } from './home.component';

@NgModule({
    imports: [CommonModule, routing],
    declarations: [HomeComponent],
    providers: []
})
export class HomeModule {
}
