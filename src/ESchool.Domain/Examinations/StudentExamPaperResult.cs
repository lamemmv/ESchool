namespace ESchool.Domain.Entities.Examinations
{
    public class StudentExamPaperResult : BaseEntity
    {
        public int QuestionId { get; set; }

        public int AnswerId { get; set; }

        public int DSS { get; set; }

        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int ExamPaperId { get; set; }

        public virtual ExamPaper ExamPaper { get; set; }
    }
}
