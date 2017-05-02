using System;
using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class QuestionViewModel
    {
        [Required]
        public string Content { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public int DifficultLevel { get; set; }

        public bool Specialized { get; set; }

        public DateTime Month { get; set; }

        public string[] QTags { get; set; }

        public AnswerViewModel[] Answers { get; set; }
    }
}
