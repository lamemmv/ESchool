using System.Collections.Generic;

namespace ESchool.Data.Entities.Examinations
{
    public class ExamPaper : BaseEntity
    {
        public string Name { get; set; }

        public int GroupId { get; set; }

        public int Duration { get; set; }

        public bool Specialized { get; set; }

        public virtual ICollection<QuestionExamPaper> QuestionExamPapers { get; set; }

        public virtual ICollection<StudentExamPaperResult> StudentExamPaperResults { get; set; }
    }
}
