import { Injectable } from '@angular/core';

@Injectable()
export class UtilitiesService {

    constructor() {

    }

    nextChar(ch: string): string {
        return String.fromCharCode(ch.charCodeAt(0) + 1);
    };  
}