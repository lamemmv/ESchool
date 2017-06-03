export class PageModel {

}

declare global {
    interface StringConstructor {
        format(...args: string[]): string;
    }
}

String.format = function (...args: string[]): string {
    var result = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        result = result.replace(reg, arguments[i + 1]);
    }
    return result;
}