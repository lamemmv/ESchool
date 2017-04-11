import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { TranslatePipe }   from './translate.pipe';

@NgModule({
    imports: [BrowserModule],
    declarations: [TranslatePipe],
    exports: [TranslatePipe]
})
export class TranslateModule { }