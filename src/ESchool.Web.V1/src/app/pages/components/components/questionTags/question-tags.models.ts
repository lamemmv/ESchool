export class QuestionTag {
    id: number;
    name: string;
    description: string;
    parentId: number;
    groupId: number;
    subQTags: QuestionTag[];
    path: string;
    parentQTags: QuestionTag[];
    subQTagsCount: number;
}