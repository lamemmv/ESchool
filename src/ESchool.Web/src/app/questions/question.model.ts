export class Question {
    id: number;
    content: string;
    description: string;
    type: number;
    qTags: QTag[] = new Array();
    answers: Answer[] = new Array();
}

export class CreateQuestionModel {
    id: number;
    content: string;
    description: string;
    type: number;
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
}