export class ExamPaper {
    id: number;
    name: string;
    duration: number;
    groupId: number;
    specialized: boolean;
    fromDate: Date;
    toDate: Date;
    exceptList: any[] = [];
    parts: any[] = [];
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