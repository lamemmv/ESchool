export class QuestionTag{
    id: number;
    name: string;
    value: string; // this is extension field for input tags.
    description: string;
    parentId: number;
    groupId: number;
    subQTags: QuestionTag[];
}