export class ExamPaper {
    id: number;
    name: string;
    duration: number;
    groupId: number;
    specialized: boolean;
    fromDate: Date;
    toDate: Date;
    exceptList: number[] = [];
    parts: ExamPaperPart[] = [];
}

export class PagedList {
    data: ExamPaper[] = new Array();
    page: number;
    size: number;
    totalItems: number;
    totalPages: number;
}

export class ModalView {
    title: string;
    okText: string;
}

export class ExamPaperPart {
    partName: string;
    qTagPath: string;
    qTagId: number;
    totalQuestion: number;
    totalGrade: number;
}