import {
    NgModule, Component, Input, Output,
    EventEmitter, AfterViewInit, OnChanges, SimpleChange,
    ViewEncapsulation
} from '@angular/core';
import { DatePipe } from '@angular/common';

@Component({
    selector: 'es-datepicker',
    providers: [DatePipe],
    templateUrl: './date-picker.component.html',
    styleUrls: [('./date-picker.style.scss')],
    encapsulation: ViewEncapsulation.None
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
        this.showDatepicker = !this.showDatepicker;
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
        if (this.showDatepicker) {
            this.close();
        }
    }

    ngAfterViewInit() {
        this.dt = new Date(this.value);
        this.value = this.transformDate(this.dt);
    }

    ngOnChanges(changes: { [propKey: string]: SimpleChange }) {
        for (let propName in changes) {
            let changedProp = changes[propName];
            if (!changedProp.isFirstChange()) {
                this.dt = new Date(changedProp.currentValue);
                this.value = this.transformDate(this.dt);
            }
        }
    }
}

