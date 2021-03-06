﻿namespace ESchool.Data.Entities.Examinations
{
    public class Answer : BaseEntity
    {
        public string AnswerName { get; set; }

        public string Body { get; set; }

        public bool DSS { get; set; }

        public int? QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
