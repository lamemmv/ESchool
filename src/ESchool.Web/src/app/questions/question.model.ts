export class Question {
    id: number;
    content: string;
    description: string;
    type: number;
    difficultLevel: number;
    qTags: QTag[] = new Array();
    answers: Answer[] = new Array();
}

export class CreateQuestionModel {
    id: number;
    content: string;
    description: string;
    type: number;
    difficultLevel: number;
    qTags: string[] = new Array();
    answers: Answer[] = new Array();
}

export class CKEditorConfig {
    uiColor: string;
}

export class QuestionView {
    title: string;
    okText: string;
    cancelText: string;
}

export class Answer {
    answerName: string;
    body: string;
    dss: boolean;
}

export class QuestionType {
    id: number;
    name: string;
};

export enum QuestionTypes {
    SingleChoice = 1,
    MultipleChoice = 2
};

export class QTag {
    id: number;
    name: string;
};

export class FormFile {
    id: number;
    type: string;
    size: number;
    name: string;
    content: any[];
};

declare global {
    interface StringConstructor {
        format(...args: string[]): string;
    }
};

String.format = function (...args: string[]): string {
    var result = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        result = result.replace(reg, arguments[i + 1]);
    }
    return result;
}