import { NgModule } from '@angular/core';
import { CommonModule }  from '@angular/common';
import { FormsModule } from '@angular/forms';

import { routing } from './home.routing.module';
import { HomeComponent } from './home.component';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        routing
    ],
    declarations: [
        HomeComponent
    ],
    providers: []
})

export class HomeModule { }