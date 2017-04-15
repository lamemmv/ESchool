using System.Collections.Generic;

namespace ESchool.Domain.Entities.Examinations
{
    public class Question : BaseEntity
    {
        public string Content { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public virtual ICollection<QuestionTag> QuestionTags { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<QuestionExamPaper> QuestionExamPapers { get; set; }
    }
}
