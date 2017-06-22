import { NgModule } from '@angular/core';
import { CommonModule }  from '@angular/common';

import { routing } from './footer.routing';
import { FooterComponent } from './footer.component';
@NgModule({
    imports: [
        CommonModule,
        routing
    ],
    declarations: [
        FooterComponent
    ],
    providers: [],
    exports: [
        FooterComponent
    ]
})

export class FooterModule { }