import {
    NgModule, Component, Input, Output,
    EventEmitter, AfterViewInit, OnChanges, SimpleChange
} from '@angular/core';
import { DatePipe } from '@angular/common';

@Component({
    selector: 'es-datepicker',
    providers: [DatePipe],
    templateUrl: './date-picker.component.html',
    styles: [`
    .dp-popup {
        position: absolute;
        padding:10px;
        background-color: #fff;
        border: 1px solid #ccc;      
        z-index:2;
    }
    .input-group.date { width:200px;}
  `]
})
export class DatepickerComponent implements AfterViewInit, OnChanges {
    public dt: Date = new Date();
    @Input() value: string;
    @Input() id: string;
    @Output() dateModelChange: EventEmitter<Date> = new EventEmitter();
    private showDatepicker: boolean = false;

    constructor(private datePipe: DatePipe) { }

    private transformDate(date: Date): string {
        var d = new DatePipe('pt-PT').transform(date, 'yyyy/MM/dd');
        return d;
    }

    today(): void {
        this.dt = new Date();
        this.apply();
        this.close();
    }
    clear(): void {
        this.dt = this.value = void 0;
        this.close();
    }

    private apply(): void {
        this.value = this.transformDate(this.dt);
        this.dateModelChange.emit(this.dt);
    }

    open() {
        this.showDatepicker = true;
    }

    close() {
        this.showDatepicker = false;
    }

    onSelectionDone(event: any) {
        this.dt = event;
        this.apply();
        this.close();
    }

    onClickedOutside(event: any) {
        if (this.showDatepicker) this.close();
    }

    ngAfterViewInit() {
        this.dt = new Date(this.value);
    }

    ngOnChanges(changes: { [propKey: string]: SimpleChange }) {
        for (let propName in changes) {
            let changedProp = changes[propName];            
            if (!changedProp.isFirstChange()) {
                this.dt = new Date(changedProp.currentValue);
            }
        }
    }
}

