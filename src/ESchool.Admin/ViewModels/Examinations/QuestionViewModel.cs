using System;

namespace ESchool.Admin.ViewModels.Examinations
{
    public class QuestionViewModel
    {
        public string Content { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public int DifficultLevel { get; set; }

        public bool Specialized { get; set; }

        public DateTime Month { get; set; }

        public int QTagId { get; set; }

        public AnswerViewModel[] Answers { get; set; }
    }
}
