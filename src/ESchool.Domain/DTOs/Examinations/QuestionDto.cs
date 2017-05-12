using System;
using System.Collections.Generic;

namespace ESchool.Domain.DTOs.Examinations
{
    public class QuestionDto
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public int DifficultLevel { get; set; }

        public bool Specialized{ get; set; }

        public DateTime Month { get; set; }

        public int QTagId { get; set; }

        public QTagDto QTag { get; set; }

        public IEnumerable<AnswerDto> Answers { get; set; }
    }
}
