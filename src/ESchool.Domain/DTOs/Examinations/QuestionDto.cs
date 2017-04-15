using System.Collections.Generic;

namespace ESchool.Domain.DTOs.Examinations
{
    public class QuestionDto
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public IEnumerable<IdNameDto> QTags { get; set; }

        public IEnumerable<AnswerDto> Answers { get; set; }
    }
}
