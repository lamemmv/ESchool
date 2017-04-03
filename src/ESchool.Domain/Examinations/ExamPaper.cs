using System.Collections.Generic;

namespace ESchool.Domain.Entities.Examinations
{
    public class ExamPaper : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<QuestionExamPaper> QuestionExamPapers { get; set; }

        public virtual ICollection<StudentExamPaperResult> StudentExamPaperResults { get; set; }
    }
}
