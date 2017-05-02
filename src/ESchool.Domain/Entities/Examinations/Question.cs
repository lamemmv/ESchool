using System;
using System.Collections.Generic;

namespace ESchool.Domain.Entities.Examinations
{
    public class Question : BaseEntity
    {
        public string Content { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public int DifficultLevel { get; set; }

        public bool Specialized { get; set; }

        public DateTime Month { get; set; }

        public int QTagId { get; set; }

        public virtual QTag QTag { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<QuestionExamPaper> QuestionExamPapers { get; set; }
    }
}
