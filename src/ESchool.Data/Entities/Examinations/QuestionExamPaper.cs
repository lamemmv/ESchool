namespace ESchool.Data.Entities.Examinations
{
    public class QuestionExamPaper
    {
        public float Grade { get; set; }

        public int Order { get; set; }

        public int ExamPaperId { get; set; }

        public virtual ExamPaper ExamPaper { get; set; }

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
