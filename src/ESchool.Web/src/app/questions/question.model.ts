import { TreeModel, RenamableNode, 
    TreeModelSettings, FoldingType, NodeEvent } from 'ng2-tree';
import {ChildrenLoadingFunction, TreeStatus } from 'ng2-tree/src/tree.types';

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
};


export class ESTreeNode implements TreeModel {
    id: number;
    value: string | RenamableNode;
    children?: ESTreeNode[];
    loadChildren?: ChildrenLoadingFunction;
    settings?: TreeModelSettings;
    _status?: TreeStatus;
    _foldingType?: FoldingType;
};