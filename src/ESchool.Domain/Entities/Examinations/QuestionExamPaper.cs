﻿namespace ESchool.Domain.Entities.Examinations
{
    public class QuestionExamPaper : BaseEntity
    {
        public string Comment { get; set; }

        public int ExamPaperId { get; set; }

        public virtual ExamPaper ExamPaper { get; set; }

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}