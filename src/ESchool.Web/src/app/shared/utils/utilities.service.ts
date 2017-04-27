import { Injectable } from '@angular/core';

@Injectable()
export class UtilitiesService {

    constructor() {

    }

    nextChar(ch: string): string {
        return String.fromCharCode(ch.charCodeAt(0) + 1);
    }

    formatString(args: any[]): string {
        var result = args[0];
        for (var i = 0; i < args.length - 1; i++) {
            var reg = new RegExp("\\{" + i + "\\}", "gm");
            result = result.replace(reg, args[i + 1]);
        }
        return result;
    };    
}