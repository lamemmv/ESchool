export class Question {
    id: number;
    content: string;
    description: string;
    type: number;
    specialized: boolean;
    difficultLevel: number;
    month: Date;
    qtagId: number;
    answers: Answer[] = new Array();
    qTag: any;
}

export class PagedList {
    data: Question[] = new Array();
    page: number;
    size: number;
    totalItems: number;
    totalPages: number;
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
}

export enum QuestionTypes {
    SingleChoice = 1,
    MultipleChoice = 2
}

export class QTag {
    id: number;
    name: string;
}

export class FormFile {
    id: number;
    type: string;
    size: number;
    name: string;
    content: any[];
}
