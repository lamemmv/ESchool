export class ExamPaper {
    id: number;
    name: string;
    questions: any[];
}

export class PagedList {
    data: ExamPaper[] = new Array();
    page: number;
    size: number;
    totalItems: number;
    totalPages: number;
}